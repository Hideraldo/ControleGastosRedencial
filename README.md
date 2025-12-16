# ControleGastosRedencial

Controle de Gastos Residencial
Um sistema completo para gerenciamento de receitas e despesas domésticas, desenvolvido com arquitetura moderna e tecnologias atuais.

📋 Descrição
O Controle de Gastos Residencial é uma aplicação web para gerenciamento financeiro doméstico, permitindo o controle de receitas e despesas por pessoa dentro de uma residência. O sistema oferece uma interface intuitiva para categorizar transações e acompanhar o fluxo financeiro familiar.

✨ Funcionalidades
✅ Cadastro de pessoas da residência

✅ Categorização de transações (receitas e despesas)

✅ Registro completo de transações financeiras

✅ Interface moderna e responsiva

✅ Banco de dados SQLite integrado

✅ API RESTful completa

✅ Desenvolvido com TypeScript para maior segurança no frontend

🏗️ Arquitetura
O projeto segue uma arquitetura cliente-servidor:

Backend (Server)
Tecnologia: C# com .NET 8

Localização: ControleGastosRedencial.Server

Banco de Dados: SQLite

API: RESTful com controllers

Frontend (Client)
Tecnologia: React com TypeScript

Localização: controlegastosredencial.client

Framework: Vite (npm run dev)

Porta: 5173


🧱 Modelos de Dados
Pessoa
Representa os moradores da residência que realizam transações.

Categoria
Organiza as transações em grupos (Alimentação, Transporte, Lazer, Salário, etc.).

Transação
Registra as movimentações financeiras, podendo ser:

Receita: Entrada de recursos

Despesa: Saída de recursos

Cada transação está associada a uma Pessoa e uma Categoria.

🚀 Como Executar o Projeto
Pré-requisitos
Visual Studio 2022 (ou superior)

.NET 8 SDK

Node.js 18+ e npm

Navegador web moderno

Passo a Passo
Clone o repositório

git clone <seu-repositorio>
cd ControleGastosRedencial

Execute o Backend (Visual Studio 2022)

Abra o projeto ControleGastosRedencial.sln no Visual Studio 2022

Configure o projeto ControleGastosRedencial.Server como projeto de inicialização

Execute o projeto (F5 ou Ctrl+F5)

O backend estará disponível em: https://localhost:7271 (ou porta similar)

Execute o Frontend

Abra um terminal

Navegue até a pasta do frontend:

cd F:\Dev\ControleGastosRedencial\controlegastosredencial.client

Instale as dependências (se necessário):

npm install

Execute o servidor de desenvolvimento:

npm run dev

Acesse a Aplicação

Abra seu navegador

Acesse: http://localhost:5173

O sistema estará pronto para uso!

🔧 Configuração do Ambiente
Banco de Dados
O SQLite é utilizado e será criado automaticamente na primeira execução

O arquivo do banco é gerado na pasta do projeto backend

Migrations são aplicadas automaticamente

Variáveis de Ambiente
O projeto utiliza configurações padrão. Para personalizações:

Backend: Edite appsettings.json

Frontend: Configure variáveis no .env (se necessário)

📚 API Endpoints
Pessoas
GET /api/pessoas - Lista todas as pessoas

GET /api/pessoas/{id} - Obtém uma pessoa específica

POST /api/pessoas - Cria uma nova pessoa

PUT /api/pessoas/{id} - Atualiza uma pessoa

DELETE /api/pessoas/{id} - Remove uma pessoa

Categorias
GET /api/categorias - Lista todas as categorias

GET /api/categorias/{id} - Obtém uma categoria específica

POST /api/categorias - Cria uma nova categoria

PUT /api/categorias/{id} - Atualiza uma categoria

DELETE /api/categorias/{id} - Remove uma categoria

Transações
GET /api/transacoes - Lista todas as transações

GET /api/transacoes/{id} - Obtém uma transação específica

POST /api/transacoes - Cria uma nova transação

PUT /api/transacoes/{id} - Atualiza uma transação

DELETE /api/transacoes/{id} - Remove uma transação

GET /api/transacoes/periodo?inicio={data}&fim={data} - Filtra por período

🧪 Testando a Aplicação
Testes Manuais
Cadastre algumas pessoas

Crie categorias de receita e despesa

Registre transações associando pessoas e categorias

Verifique os relatórios e saldos

🤝 Contribuindo
Faça um Fork do projeto

Crie uma Branch para sua Feature (git checkout -b feature/AmazingFeature)

Commit suas mudanças (git commit -m 'Add some AmazingFeature')

Push para a Branch (git push origin feature/AmazingFeature)

Abra um Pull Request

📄 Licença
Este projeto está licenciado sob a licença MIT - veja o arquivo LICENSE para detalhes.

🆘 Suporte
Se encontrar problemas:

Verifique se todas as dependências estão instaladas

Confirme se ambas as aplicações (backend e frontend) estão rodando

Verifique as portas utilizadas (backend: ~7271, frontend: 5173)

Consulte os logs no console de cada aplicação

✍️ Autor: Hideraldo L. Rondon
Desenvolvido como projeto de controle financeiro residencial.

Aviso: Este é um projeto para fins educacionais e de portfólio. Use como referência para seus próprios projetos.

