# Controle de Gastos Residencial

Sistema completo para gerenciamento de receitas e despesas domésticas, desenvolvido com **arquitetura moderna** e **tecnologias atuais**, focado em organização financeira familiar.

---

## 📋 Descrição

O **Controle de Gastos Residencial** é uma aplicação web para gerenciamento financeiro doméstico, permitindo o controle de **receitas e despesas por pessoa** dentro de uma residência.

O sistema oferece uma interface **intuitiva e responsiva**, facilitando a categorização de transações e o acompanhamento do fluxo financeiro familiar.

---

## ✨ Funcionalidades

* ✅ Cadastro de pessoas da residência
* ✅ Categorização de transações (receitas e despesas)
* ✅ Registro completo de transações financeiras
* ✅ Interface moderna e responsiva
* ✅ Banco de dados **SQLite** integrado
* ✅ **API RESTful** completa
* ✅ Frontend desenvolvido com **TypeScript**, garantindo maior segurança e tipagem

---

## 🏗️ Arquitetura

O projeto segue o padrão **cliente-servidor**, com separação clara entre backend e frontend.

### 🔧 Backend (Server)

* **Tecnologia:** C# com .NET 8
* **Localização:** `ControleGastosRedencial.Server`
* **Banco de Dados:** SQLite
* **API:** RESTful utilizando Controllers

### 🎨 Frontend (Client)

* **Tecnologia:** React com TypeScript
* **Localização:** `controlegastosredencial.client`
* **Framework de build:** Vite
* **Comando de execução:** `npm run dev`
* **Porta padrão:** `5173`

---

## 🧱 Modelos de Dados

### 👤 Pessoa

Representa os moradores da residência que realizam transações financeiras.

### 🗂️ Categoria

Organiza as transações em grupos, como:

* Alimentação
* Transporte
* Lazer
* Salário
* Outros

### 💰 Transação

Registra as movimentações financeiras da residência, podendo ser:

* **Receita:** Entrada de recursos
* **Despesa:** Saída de recursos

Cada transação está associada a uma **Pessoa** e a uma **Categoria**.

---

## 🚀 Como Executar o Projeto

### 📌 Pré-requisitos

* Visual Studio 2022 (ou superior)
* .NET 8 SDK
* Node.js 18+ e npm
* Navegador web moderno

---

### ▶️ Passo a Passo

#### 1️⃣ Clonar o repositório

```bash
git clone
cd ControleGastosRedencial
```

---

#### 2️⃣ Executar o Backend

1. Abra o arquivo `ControleGastosRedencial.sln` no **Visual Studio 2022**
2. Defina o projeto `ControleGastosRedencial.Server` como **Startup Project**
3. Execute o projeto (`F5` ou `Ctrl + F5`)

📡 O backend estará disponível em:

```
https://localhost:7271
```

*(a porta pode variar)*

---

#### 3️⃣ Executar o Frontend

1. Abra um terminal
2. Navegue até a pasta do frontend:

```bash
cd F:\Dev\ControleGastosRedencial\controlegastosredencial.client
```

3. Instale as dependências:

```bash
npm install
```

4. Inicie o servidor de desenvolvimento:

```bash
npm run dev
```

---

### 🌐 Acessar a Aplicação

Abra o navegador e acesse:

```
http://localhost:5173
```

O sistema estará pronto para uso 🚀

---

## 🔧 Configuração do Ambiente

### 🗄️ Banco de Dados

* O **SQLite** é utilizado como banco de dados
* O banco é criado automaticamente na primeira execução
* O arquivo é gerado na pasta do projeto backend
* As **migrations** são aplicadas automaticamente

### ⚙️ Variáveis de Ambiente

O projeto utiliza configurações padrão. Para personalizações:

* **Backend:** editar o arquivo `appsettings.json`
* **Frontend:** configurar variáveis no arquivo `.env` (se necessário)

---

## 📚 API Endpoints

### 👤 Pessoas

* `GET /api/pessoas` – Lista todas as pessoas
* `GET /api/pessoas/{id}` – Obtém uma pessoa específica
* `POST /api/pessoas` – Cria uma nova pessoa
* `PUT /api/pessoas/{id}` – Atualiza uma pessoa
* `DELETE /api/pessoas/{id}` – Remove uma pessoa

---

### 🗂️ Categorias

* `GET /api/categorias` – Lista todas as categorias
* `GET /api/categorias/{id}` – Obtém uma categoria específica
* `POST /api/categorias` – Cria uma nova categoria
* `PUT /api/categorias/{id}` – Atualiza uma categoria
* `DELETE /api/categorias/{id}` – Remove uma categoria

---

### 💰 Transações

* `GET /api/transacoes` – Lista todas as transações
* `GET /api/transacoes/{id}` – Obtém uma transação específica
* `POST /api/transacoes` – Cria uma nova transação
* `PUT /api/transacoes/{id}` – Atualiza uma transação
* `DELETE /api/transacoes/{id}` – Remove uma transação
* `GET /api/transacoes/periodo?inicio={data}&fim={data}` – Filtra transações por período

---

## 🧪 Testando a Aplicação

### Testes Manuais

1. Cadastre algumas pessoas
2. Crie categorias de receita e despesa
3. Registre transações associando pessoas e categorias
4. Verifique os saldos e o fluxo financeiro

---

## 🤝 Contribuindo

Contribuições são bem-vindas!

1. Faça um **Fork** do projeto
2. Crie uma branch para sua feature:

```bash
git checkout -b feature/AmazingFeature
```

3. Commit suas mudanças:

```bash
git commit -m "Add some AmazingFeature"
```

4. Faça o push para a branch:

```bash
git push origin feature/AmazingFeature
```

5. Abra um **Pull Request**

---

## 📄 Licença

Este projeto está licenciado sob a licença **MIT**. Consulte o arquivo `LICENSE` para mais detalhes.

---

## 🆘 Suporte

Se encontrar problemas:

* Verifique se todas as dependências estão instaladas
* Confirme se backend e frontend estão em execução
* Verifique as portas utilizadas:

  * Backend: ~7271
  * Frontend: 5173
* Consulte os logs no console de cada aplicação

---

## ✍️ Autor

**Hideraldo L. Rondon**
Projeto desenvolvido como estudo e portfólio para controle financeiro residencial.

---

> ⚠️ **Aviso:** Este é um projeto com fins educacionais e de portfólio. Use como referência para seus próprios projetos.
