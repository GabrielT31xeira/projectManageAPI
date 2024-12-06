# Projeto de Gerenciamento de Tarefas e Equipes

Este projeto � uma API para gerenciar tarefas, equipes, projetos e usu�rios. A API fornece endpoints para criar, atualizar, excluir e listar itens, al�m de gerenciar arquivos e coment�rios associados �s tarefas.

## Funcionalidades Principais
- Gerenciamento de tarefas, projetos, equipes e usu�rios.
- Controle de arquivos associados a tarefas.
- Sistema de coment�rios em tarefas.
- Autentica��o e registro de usu�rios.
- Controle de participa��o em equipes.

---

## Endpoints da API

### **Arquivo**
- **Adicionar Arquivo a uma Tarefa**  
  `POST /api/Arquivo/{tarefaId}/tarefa`  
  - **Par�metros**: `tarefaId` (ID da tarefa)  
  - **Body**: `ArquivoCreateModel`  

- **Obter Arquivos de uma Tarefa**  
  `GET /api/Arquivo/{tarefaId}/tarefa`  
  - **Par�metros**: `tarefaId` (ID da tarefa)  

- **Listar Todas as Tarefas com Arquivos Associados**  
  `GET /api/Arquivo`  

- **Excluir Arquivo pelo ID**  
  `DELETE /api/Arquivo/{arquivoId}`  
  - **Par�metros**: `arquivoId` (ID do arquivo)  

---

### **Autentica��o**
- **Registrar Usu�rio**  
  `POST /api/Auth/register`  
  - **Body**: `RegisterModel`  

- **Confirmar E-mail**  
  `GET /api/Auth/confirmemail`  
  - **Par�metros**: `userId`, `token`  

- **Login de Usu�rio**  
  `POST /api/Auth/login`  
  - **Body**: `LoginModel`  

---

### **Coment�rios**
- **Adicionar Coment�rio a uma Tarefa**  
  `POST /api/Comentario/{tarefaId}`  
  - **Par�metros**: `tarefaId`  
  - **Body**: `ComentarioCreateModel`  

- **Remover Coment�rio**  
  `DELETE /api/Comentario/{comentarioId}`  
  - **Par�metros**: `comentarioId`  

- **Listar Coment�rios de uma Tarefa**  
  `GET /api/Comentario/{tarefaId}/comentarios`  
  - **Par�metros**: `tarefaId`  

---

### **Equipes**
- **Entrar em uma Equipe**  
  `POST /api/Equipes/EntrarNaEquipe`  
  - **Body**: `EntrarNaEquipeModel`  

- **Sair de uma Equipe**  
  `POST /api/Equipes/SairDaEquipe`  
  - **Body**: `SairDaEquipeModel`  

- **Listar Equipes**  
  `GET /api/Equipes`  

- **Obter Detalhes de uma Equipe**  
  `GET /api/Equipes/{id}`  
  - **Par�metros**: `id`  

- **Criar Equipe**  
  `POST /api/Equipes/Create`  
  - **Body**: `EquipeCreateModel`  

- **Editar Equipe**  
  `PUT /api/Equipes/Edit/{id}`  
  - **Par�metros**: `id`  
  - **Body**: `EquipeEditModel`  

- **Excluir Equipe**  
  `DELETE /api/Equipes/Delete/{id}`  
  - **Par�metros**: `id`  

- **Remover Usu�rio de uma Equipe**  
  `POST /api/Equipes/RemoverUsuario`  
  - **Body**: `RemoverUsuarioModel`  

---

### **Projetos**
- **Listar Projetos**  
  `GET /api/Projeto`  

- **Criar Novo Projeto**  
  `POST /api/Projeto`  
  - **Body**: `ProjetoCreateModel`  

- **Obter Projeto por ID**  
  `GET /api/Projeto/{id}`  
  - **Par�metros**: `id`  

- **Editar Projeto**  
  `PUT /api/Projeto/{id}`  
  - **Par�metros**: `id`  
  - **Body**: `ProjetoEditModel`  

- **Excluir Projeto**  
  `DELETE /api/Projeto/{id}`  
  - **Par�metros**: `id`  

---

### **Tarefas**
- **Listar Tarefas de um Projeto**  
  `GET /api/Tarefas`  
  - **Par�metros**: `projetoId`  

- **Obter Tarefa por ID**  
  `GET /api/Tarefas/{id}`  
  - **Par�metros**: `id`  

- **Excluir Tarefa**  
  `DELETE /api/Tarefas/{id}`  
  - **Par�metros**: `id`  

- **Criar Nova Tarefa para um Projeto**  
  `POST /api/Tarefas/{projetoId}`  
  - **Par�metros**: `projetoId`  
  - **Body**: `TarefaCreateModel`  

---

## Tecnologias Utilizadas
- **Backend**: ASP.NET Core  
- **Banco de Dados**: SQL Server  
- **Autentica��o**: Identity  
- **Gerenciamento de Dados**: Entity Framework Core  

---

## Como Executar o Projeto
1. Clone este reposit�rio.  
2. Configure as vari�veis de ambiente e banco de dados no arquivo `appsettings.json` e rode `Update-Database`.  
3. Execute o comando `dotnet run` para iniciar o servidor.  
4. Utilize ferramentas como Postman ou Swagger para testar os endpoints. (swagger: http://localhost:5110/index.html)  
5. No arquivo \API.projectManager\Controllers\AuthController.cs substituir os dados do mailTrap na fun��o "SendConfirmationEmail" 
- ![UML](mailtrap.jpg)
- ![UML](uml.png)
---

## Licen�a
Este projeto est� licenciado sob a [MIT License](LICENSE).
