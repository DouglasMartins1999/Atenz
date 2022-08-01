# **Atenz**

O **Atenz** - derivado de Atena, deusa do conhecimento na mitologia grega - é uma web biblioteca online para streaming de cursos e livros didáticos e educacionais. Por meio dela, os usuários podem criar um perfil para armazenar sua atividade, favoritos, próximos passos e visualizar indicadores sobre seu progresso. Uma versão de demonstração pode ser encontrada em atenz.dotins.eu.org. Utilize “demo” como usuário e senha para acessar um perfil modelo.

![Atenz](https://i.imgur.com/VYrM8pa.jpg)




## Tecnicamente...

Seu backend foi desenvolvido em C# com .NET Core 5.1 e Entity Framework, estruturando seus dados no PostgreSQL. No frontend, a aplicação foi construída usando Angular 9. Foi totalmente desenvolvido em menos de 2 meses. Graças ao Plyr, que equipa o Atenz, permite suporte a vídeos tanto do YouTube quanto do Vimeo. O backend também oferece a possibilidade de armazenar livros e vídeos no Amazon S3 (e similares), gerando links temporários para cada visualização, ou também usar links direto ao arquivo.

Sendo um projeto web responsivo, o Atenz está disponível a qualquer desktop, notebook, tablet ou celular conectado a internet, bastando apenas um navegador para acessar seu conteúdo de qualquer lugar. Como um Progressive Web App (PWA), o Atenz também pode ser instalado em dispositivos Android, similar a uma aplicação nativa.

![AtenzMobile](https://i.imgur.com/SnbRBJ0.jpg)


## Como implantar um pra mim?

> TO DO...

Confira as imagens no DockerHub para o [client em Angular](https://hub.docker.com/r/martindoug/atenz-app) e o [server em .NET](https://hub.docker.com/r/martindoug/atenz-service)
