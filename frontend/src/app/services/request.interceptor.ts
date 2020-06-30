import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError, map, shareReplay } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { ModalService, ModalData } from './modal.service';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { LoadingService } from './loading.service';
import { AuthService } from './auth.service';

@Injectable()
export class RequestInterceptor implements HttpInterceptor {
    constructor(
        private authService: AuthService,
        private modalService: ModalService,
        private loadingService: LoadingService,
        private router: Router
    ) {}

    intercept(req: HttpRequest<any>, next: HttpHandler){
        const token: string = this.authService.retrieveToken();
        this.loadingService.changeVisibility(true);
        
        req = req.clone({
            setHeaders: {
                "Authorization": "Bearer " + token
            }
        })

        return next.handle(req)
            .pipe(
                map(item => {
                    this.loadingService.changeVisibility(false);
                    return item;
                }),
                shareReplay(1),
                catchError((err) => {
					const modaldata = new ModalData()
						.fromService(this.modalService)
						.setTitle("Oops...")
						.setDescription("Aparentemente, não conseguimos processar sua solicitação no momento. Por favor, tente mais tarde")
						.setCloseAction()
						.setVisibility(true);
	
					if(err instanceof HttpErrorResponse){
						if(err.status == 403){
							modaldata.setDescription("Não foi possível encontrar o usuário informado ou a senha preenchida não está correta. Confira os dados inseridos e tente novamente.")
                        }
                        
                        if(err.status == 401){
                            this.authService.signout();
                            modaldata.setDescription("Sua sessão expirou, faça login novamente para continuar utilizando nossos serviços");
                        }
					}
    
                    this.modalService.changeModalContent(modaldata);
					return throwError(err);
				})
            )
    }
}

export const RequestInterceptorConfig = {
    provide: HTTP_INTERCEPTORS,
    useClass: RequestInterceptor,
    multi: true
}