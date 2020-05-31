import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalService, ModalData } from 'src/app/services/modal.service';
import { Router } from '@angular/router';

@Component({
  selector: 'atz-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public formLogin: FormGroup;

  constructor(
    private auth: AuthService, 
    private formBuilder: FormBuilder,
    private modalService: ModalService,
    private router: Router) { }

  ngOnInit(): void {
    this.formLogin = this.formBuilder.group({
        username: this.formBuilder.control('', [Validators.required]),
        password: this.formBuilder.control('', [Validators.required])
    })
  }

  login(){
    const modal = new ModalData()
      .fromService(this.modalService)
      .setVisibility(true)
      .setTitle("Oops...")
      .setCloseAction();

    if(!this.formLogin.valid){
      const { controls } = this.formLogin;

      if(controls.password?.errors?.required){
        modal.setDescription("Precisamos de sua senha para fazer login. Caso tenha esquecido, pode recuperá-la em \"Esqueci Minha Senha\"");
      }
      
      if(controls.username?.errors?.required){
        modal.setDescription("O nome de usuário deve ser preenchido");
      }
      
      return this.modalService.changeModalContent(modal);
    }

    const { username, password } = this.formLogin.value;
    this.auth.signin(username, password)
      .subscribe(data => this.router.navigate(['/']))
  }

}
