- ## Sobre o projeto

  [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=marcoservio22_MyRecipeBook&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=marcoservio22_MyRecipeBook) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=marcoservio22_MyRecipeBook&metric=bugs)](https://sonarcloud.io/summary/new_code?id=marcoservio22_MyRecipeBook)[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=marcoservio22_MyRecipeBook&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=marcoservio22_MyRecipeBook)  [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=marcoservio22_MyRecipeBook&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=marcoservio22_MyRecipeBook) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=marcoservio22_MyRecipeBook&metric=coverage)](https://sonarcloud.io/summary/new_code?id=marcoservio22_MyRecipeBook) [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=marcoservio22_MyRecipeBook&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=marcoservio22_MyRecipeBook) 

  Apresentando o **Meu Livro de Receitas** - uma aplicação para quem adora cozinhar e compartilhar receitas! O Meu Livro de Receitas foi projetado para tornar sua vida na cozinha mais fácil, ajudando você a se organizar, gerenciar suas receitas e tornar sua experiência culinária mais agradável.

  Este projeto consiste em uma **API** desenvolvida em **.NET** para o gerenciamento de receitas culinárias. A **API** permite que os usuários se cadastrem fornecendo nome, e-mail e senha. Após o cadastro, os usuários podem criar, editar, filtrar e deletar receitas. Cada receita deve incluir um título, ingredientes e instruções. Adicionalmente, os usuários têm a opção de adicionar o tempo de preparo, nível de dificuldade e uma imagem ilustrativa à receita.

  A **API** oferece suporte para **MySQL** e **SQLServer** como opções de banco de dados, proporcionando flexibilidade na escolha do ambiente de armazenamento de dados. A configuração de pipelines **CI/CD** e a integração com **Sonarcloud** garantem uma análise contínua do código, promovendo um desenvolvimento mais robusto e seguro.

  Seguindo os princípios de **Domain-Driven Design (DDD)** e **SOLID**, a arquitetura do projeto busca manter um design modular e sustentável. A validação dos dados é realizada utilizando **FluentValidation**, assegurando que todas as entradas de dados atendam aos critérios estabelecidos.

  Para garantir a qualidade do código, são implementados **testes de unidade e de integração**. A utilização de **injeção de dependências** promove uma melhor modularidade e testabilidade do código, facilitando a manutenção e evolução do projeto.

  Outras tecnologias e práticas adotadas incluem o **Entity Framework** para o mapeamento objeto-relacional, a metodologia ágil **SCRUM** para o gerenciamento do projeto, e a implementação de **Tokens JWT & Refresh Token** para autenticação segura. As migrações do banco de dados são gerenciadas para assegurar uma evolução controlada do esquema de dados, **Docker** para conteinerizar a aplicação e facilitar o deploy no Azure. Além disso, o uso de **Git** e a estratégia de ramificação **GitFlow** auxiliam na organização e controle das versões do código.

  

  ### Features

  - **Gerenciamento de Receitas**: Criação, edição, exclusão e filtro de receitas. 🍲✏️🗑️🔍
  - **Login com Google**: Integração para autenticação via conta Google. 🔑🔗🟦
  - **Integração com ChatGPT**: Utilização de IA para melhorar a experiência dos usuários na geração de receitas a partir de ingredientes fornecidos. 🤖🍳
  - **Mensageria**: Utilização de mensageria (Service Bus - Queue), para gerenciar a exclusão de contas. 📩🗂️🚫
  - **Upload de Imagem**: Permite aos usuários enviar uma imagem para ilustrar suas receitas. 📸⬆️🖼️

  

  ### Construído com

  [![badge-dot-net](https://camo.githubusercontent.com/8322e300a264275ca24ecb11a1f24df8cd875bb14b156dd705346da0a48acf4b/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f2e4e45542d3531324244343f6c6f676f3d646f746e6574266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/8322e300a264275ca24ecb11a1f24df8cd875bb14b156dd705346da0a48acf4b/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f2e4e45542d3531324244343f6c6f676f3d646f746e6574266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-windows](https://camo.githubusercontent.com/63f7de79e2da59835723383dc3c4f60be208025841f733308cd85d9415183e45/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f57696e646f77732d3030373844343f6c6f676f3d77696e646f7773266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/63f7de79e2da59835723383dc3c4f60be208025841f733308cd85d9415183e45/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f57696e646f77732d3030373844343f6c6f676f3d77696e646f7773266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-visual-studio](https://camo.githubusercontent.com/a032578a6fa8c4f0d9a18c780ddf91070ec88015670bee813d61417f09cec7af/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f56697375616c25323053747564696f2d3543324439313f6c6f676f3d76697375616c73747564696f266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/a032578a6fa8c4f0d9a18c780ddf91070ec88015670bee813d61417f09cec7af/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f56697375616c25323053747564696f2d3543324439313f6c6f676f3d76697375616c73747564696f266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-mysql](https://camo.githubusercontent.com/d6269c76c94c7455a71c49bc85e1804072d9a42ec309559fc2979b59f8699b9e/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f4d7953514c2d3434373941313f6c6f676f3d6d7973716c266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/d6269c76c94c7455a71c49bc85e1804072d9a42ec309559fc2979b59f8699b9e/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f4d7953514c2d3434373941313f6c6f676f3d6d7973716c266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-sqlserver](https://camo.githubusercontent.com/2583596bc5f79bed331fb5466109c3ee28d4b36d31ef37ebf3002434969e6a54/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f4d6963726f736f667425323053514c2532305365727665722d4343323932373f6c6f676f3d6d6963726f736f667473716c736572766572266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/2583596bc5f79bed331fb5466109c3ee28d4b36d31ef37ebf3002434969e6a54/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f4d6963726f736f667425323053514c2532305365727665722d4343323932373f6c6f676f3d6d6963726f736f667473716c736572766572266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-swagger](https://camo.githubusercontent.com/49dbc944e59229a0a8c56e8787f20802fee3cd550b07a935e15ebb63d58e7072/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f537761676765722d3835454132443f6c6f676f3d73776167676572266c6f676f436f6c6f723d303030267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/49dbc944e59229a0a8c56e8787f20802fee3cd550b07a935e15ebb63d58e7072/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f537761676765722d3835454132443f6c6f676f3d73776167676572266c6f676f436f6c6f723d303030267374796c653d666f722d7468652d6261646765) [![badge-docker](https://camo.githubusercontent.com/1fcd9b44f82943004dbc0d3b6597e68bad27accc5e4bb19a758a18294653870d/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f446f636b65722d3234393645443f6c6f676f3d646f636b6572266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/1fcd9b44f82943004dbc0d3b6597e68bad27accc5e4bb19a758a18294653870d/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f446f636b65722d3234393645443f6c6f676f3d646f636b6572266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-azure-devops](https://camo.githubusercontent.com/3697974ef6235d2be07b2e5e4f1b954d9a8e69c0cc21cbe6c240d2d8f07a615b/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f417a7572652532304465764f70732d3030373844373f6c6f676f3d617a7572656465766f7073266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/3697974ef6235d2be07b2e5e4f1b954d9a8e69c0cc21cbe6c240d2d8f07a615b/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f417a7572652532304465764f70732d3030373844373f6c6f676f3d617a7572656465766f7073266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-azure](https://camo.githubusercontent.com/21d7367ab84c97ce9bee0709da2bf34369f6af1d7facd829f784fd328bc29985/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f4d6963726f736f6674253230417a7572652d3030373844343f6c6f676f3d6d6963726f736f6674617a757265266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/21d7367ab84c97ce9bee0709da2bf34369f6af1d7facd829f784fd328bc29985/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f4d6963726f736f6674253230417a7572652d3030373844343f6c6f676f3d6d6963726f736f6674617a757265266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-azure-pipelines](https://camo.githubusercontent.com/a418223f5d801a395ef3fd7e6f4ddfb0fa4446d81565029bca76885e82b930de/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f417a757265253230506970656c696e65732d3235363045303f6c6f676f3d617a757265706970656c696e6573266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/a418223f5d801a395ef3fd7e6f4ddfb0fa4446d81565029bca76885e82b930de/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f417a757265253230506970656c696e65732d3235363045303f6c6f676f3d617a757265706970656c696e6573266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-google](https://camo.githubusercontent.com/531f9afa46a27b070098472b6b2f743ed56cb408e3c04914778736c745c518e8/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f476f6f676c652d3432383546343f6c6f676f3d676f6f676c65266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/531f9afa46a27b070098472b6b2f743ed56cb408e3c04914778736c745c518e8/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f476f6f676c652d3432383546343f6c6f676f3d676f6f676c65266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-openai](https://camo.githubusercontent.com/fecc72a477e6f195cc379356649578443d980b1b021d0d1d66d1d2a04c445a31/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f4f70656e41492d3431323939313f6c6f676f3d6f70656e6169266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/fecc72a477e6f195cc379356649578443d980b1b021d0d1d66d1d2a04c445a31/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f4f70656e41492d3431323939313f6c6f676f3d6f70656e6169266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765) [![badge-sonarcloud](https://camo.githubusercontent.com/9a6f49df6384d7fdaa92f2518351623f18eb7738391fbf6ae07e277feb2b4286/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f536f6e6172436c6f75642d4633373032413f6c6f676f3d736f6e6172636c6f7564266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)](https://camo.githubusercontent.com/9a6f49df6384d7fdaa92f2518351623f18eb7738391fbf6ae07e277feb2b4286/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f536f6e6172436c6f75642d4633373032413f6c6f676f3d736f6e6172636c6f7564266c6f676f436f6c6f723d666666267374796c653d666f722d7468652d6261646765)

  

  ## Getting Started

  Para obter uma cópia local funcionando, siga estes passos simples.

  

  ### Requisitos

  - Visual Studio versão 2022+ ou Visual Studio Code
  - Windows 10+ ou Linux/MacOS com [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) instalado
  - MySql Server ou SqlServer

  

  ### Instalação

  1. Clone o repositório:

     ```
     git clone https://github.com/marcoservio/my-recipe-book.git
     ```

     

  2. Preencha as informações no arquivo `appsettings.Development.json`.

  3. Execute a API e aproveite o seu teste :)

  

  ## License

  Sinta-se à vontade para usar este projeto para estudar e aprender. No entanto, a distribuição ou comercialização não é permitida.