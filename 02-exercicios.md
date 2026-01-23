# Exercícios de Models, DTOs e Wrappers

## 1. Rota de categorias de produtos bancários

Neste exercício vamos continuar a implementação do recurso de categorias, conforme as user stories e casos de teste criados nos tópicos anteriores.

- Importe o _boilerplate_ de projeto **Cliente** fornecido na pasta da aula e o substitua na sua **solução Cliente-Servidor**. Esse boilerplate já tem a maior parte da implementação do "frontend" de categorias. Vamos focar na implementação do servidor e no final fazer alguns ajustes no cliente;
- No projeto **Servidor**, implemente todo o necessário para que o recurso de categorias funcione conforme descrito nas especificações:
<!-- Boilerplate em [./_assets/02-exercicio1-boilerplate-cliente/] -->

### US1 - Inclusão de nova categoria

```gherkin
US1 - Inclusão de nova categoria

Como um analista de marketing
Eu quero incluir uma nova categoria informando seu código numérico e sua descrição
Para que o banco possa manter um cadastro oficial e padronizado de categorias utilizadas por diversas áreas

Regras
    - O código numérico é obrigatório
    - O código numérico é de livre escolha
    - O código numérico deve ser único na base
    - A descrição é obrigatória
    - A descrição deve conter apenas letras, números e acentuação

Critérios de Aceitação
    - Dado que informo um código numérico ainda não utilizado e uma descrição válida, então a aplicação inclui a categoria com sucesso
    - Dado que o código numérico não é informado ou já existe na base, então a aplicação rejeita a inclusão
    - Dado que a descrição não é informada ou é inválida, então a aplicação rejeita a inclusão
```

```md
TC1 - Inclusão de categoria bem sucedida
User Story: US1 - Inclusão de nova categoria

Pré-condições
    - Não existe na base uma categoria cujo código numérico corresponde ao código da categoria a ser incluída

Entradas/Ações
    - Informar um código numérico
    - Informar uma descrição válida (apenas letras, números e acentuação)
    - Acionar a inclusão

Resultados Esperados
    - A aplicação inclui a nova categoria na base e exibe mensagem de sucesso

Pós-condições
    - A nova categoria passa a existir na base
```

```md
TC2 - Inclusão de categoria rejeitada devido a código numérico não informado
User Story: US1 - Inclusão de nova categoria

Pré-condições
    -

Entradas/Ações
    - Não informar um código numérico
    - Informar uma descrição válida (apenas letras, números e acentuação)
    - Acionar a inclusão

Resultados Esperados
    - A aplicação rejeita a inclusão e exibe mensagem informando que o código numérico é inválido

Pós-condições
    - Nenhuma nova categoria é incluída
```

```md
TC3 - Inclusão de categoria rejeitada devido a código numérico já existente
User Story: US1 - Inclusão de nova categoria

Pré-condições
    - Já existe na base uma categoria cujo código numérico corresponde ao código da categoria a ser incluída

Entradas/Ações
    - Informar um código numérico
    - Informar uma descrição válida (apenas letras, números e acentuação)
    - Acionar a inclusão

Resultados Esperados
    - A aplicação rejeita a inclusão e exibe mensagem informando que já existe uma categoria com o código numérico

Pós-condições
    - Nenhuma nova categoria é incluída
```

```md
TC4 - Inclusão de categoria rejeitada devido a descrição não informada ou inválida
User Story: US1 - Inclusão de nova categoria

Pré-condições
    -

Entradas/Ações
    - Informar um código numérico
    - Não informar uma descrição ou informar uma descrição inválida
    - Acionar a inclusão

Resultados Esperados
    - A aplicação rejeita a inclusão e exibe mensagem informando que a descrição é inválida

Pós-condições
    - Nenhuma nova categoria é incluída
```

### US2 - Remoção de categoria

```gherkin
US2 - Remoção de categoria

Como um analista comercial
Eu quero remover uma categoria previamente cadastrada
Para evitar que equipes selecionem categorias obsoletas no cadastro de novos produtos

Regras
    - A remoção deve eliminar a categoria fisicamente da base
    - A remoção deve se dar pela seleção por código numérico

Critérios de Aceitação
    - Dado que informo o código numérico de uma categoria existente, então a aplicação remove a categoria com sucesso
    - Dado que o código numérico informado não existe na base, então a aplicação rejeita a remoção e informa que a categoria não foi encontrada
```

```md
TC5 - Remoção de categoria bem sucedida
User Story: US2 - Remoção de categoria

Pré-condições  
    - Já existe na base uma categoria cujo código numérico corresponde ao código da categoria a ser removida

Entradas/Ações  
    - Informar um código numérico
    - Acionar a remoção

Resultados Esperados  
    - A aplicação remove a categoria da base e exibe mensagem de sucesso

Pós-condições  
    - A categoria deixa de existir na base
```

```md
TC6 - Remoção de categoria rejeitada devido a código numérico inexistente
User Story: US2 - Remoção de categoria

Pré-condições  
    - Não existe na base uma categoria cujo código numérico corresponde ao código da categoria a ser removida

Entradas/Ações  
    - Informar um código numérico
    - Acionar a remoção

Resultados Esperados  
    - A aplicação rejeita a remoção e exibe mensagem informando que não existe uma categoria com o código numérico

Pós-condições  
    - Nenhuma categoria é removida
```

### US3 - Alteração de dados de categoria

```gherkin
US3 - Alteração de dados de categoria

Como um gerente de marketing
Eu quero alterar a descrição de uma categoria já cadastrada
Para corrigir informações incompletas ou ajustá-la às estratégias atuais de marketing

Regras
    - Apenas a descrição pode ser alterada
    - O código numérico não pode ser modificado
    - A nova descrição é obrigatória
    - A nova descrição deve conter apenas letras, números e acentuação
    - A alteração deve registrar informações de auditoria: usuário responsável e data/hora atuais

Critérios de Aceitação
    - Dado que informo uma nova descrição válida, então a aplicação altera a categoria e registra as informações de auditoria com sucesso
    - Dado que o código numérico informado não existe na base, então a aplicação rejeita a alteração e informa que a categoria não foi encontrada
    - Dado que a nova descrição não é informada ou é inválida, então a aplicação rejeita a alteração e informa que a descrição é inválida
```

```md
TC7 - Alteração de categoria bem sucedida
User Story: US3 - Alteração de dados de categoria

Pré-condições
    - Já existe na base uma categoria cujo código numérico corresponde ao código da categoria a ser alterada

Entradas/Ações
    - Informar um código numérico
    - Informar uma nova descrição válida (apenas letras, números e acentuação)
    - Acionar a alteração

Resultados Esperados
    - A aplicação altera a categoria na base com a nova descrição, com os dados de auditoria (usuário responsável e data/hora atuais) e exibe mensagem de sucesso

Pós-condições
    - A categoria passa a ter a nova descrição e as novas informações de auditoria na base
```

```md
TC8 - Alteração de categoria rejeitada devido a código numérico inexistente
User Story: US3 - Alteração de dados de categoria

Pré-condições
    - Não existe na base uma categoria cujo código numérico corresponde ao código da categoria a ser alterada

Entradas/Ações
    - Informar um código numérico
    - Informar uma nova descrição válida (apenas letras, números e acentuação)
    - Acionar a alteração

Resultados Esperados
    - A aplicação rejeita a alteração e exibe mensagem informando que não existe uma categoria com o código numérico

Pós-condições
    - Nenhuma categoria é alterada
```

```md
TC9 - Alteração de categoria rejeitada devido a descrição não informada ou inválida
User Story: US3 - Alteração de dados de categoria

Pré-condições
    -

Entradas/Ações
    - Informar um código numérico
    - Não informar uma nova descrição ou informar uma nova descrição inválida
    - Acionar a alteração

Resultados Esperados
    - A aplicação rejeita a alteração e exibe mensagem informando que a descrição é inválida

Pós-condições
    - Nenhuma categoria é alterada
```

### US4 - Listagem de categorias

```gherkin
US4 - Listagem de categorias

Como um gerente de marketing
Eu quero visualizar a lista de todas as categorias cadastradas
Para apoiar a criação de campanhas, análises de portfólio e decisões estratégicas

Regras
    - A listagem deve retornar todas as categorias cadastradas
    - A listagem deve exibir: código numérico, descrição e informações de auditoria (última alteração)

Critérios de Aceitação
    - Ao solicitar a listagem, a aplicação retorna todas as categorias existentes com suas informações
    - Se não houver categorias cadastradas, então a aplicação rejeita a consulta e informa que a lista está vazia
```

```md
TC10 - Listagem de categorias bem sucedida
User Story: US4 - Listagem de categorias

Pré-condições  
    - Já existem na base categorias

Entradas/Ações  
    - Acionar a listagem

Resultados Esperados  
    - A aplicação lista todas as categorias
    - Para cada categoria são exibidos: código numérico, descrição e informações de auditoria (última alteração)

Pós-condições  
    -
```

```md
TC11 - Listagem de categorias vazia
User Story: US4 - Listagem de categorias

Pré-condições  
    - Não existem na base categorias

Entradas/Ações  
    - Acionar a listagem

Resultados Esperados  
    - A aplicação rejeita a listagem e exibe mensagem informando que não existem categorias

Pós-condições  
    -
```

### US5 - Consulta de categoria por código ou descrição

```gherkin
US5 - Consulta de categoria por código ou descrição

Como um gerente de produtos digitais
Eu quero consultar uma categoria pelo seu código numérico ou pela descrição
Para validar rapidamente informações em reuniões e decisões com áreas internas e parceiros

Regras
    - A consulta pode ser realizada pelo código numérico ou pela descrição exata
    - A consulta deve retornar exatamente 1 categoria caso cadastrada
    - A consulta deve retornar: código numérico, descrição e informações de auditoria (última alteração)
    - Caso não exista categoria correspondente, a aplicação deve informar inexistência

Critérios de Aceitação
    - Dado que informo um código numérico válido de uma categoria existente, então a aplicação retorna seus dados completos com sucesso
    - Dado que informo uma descrição exata de uma categoria existente, então a aplicação retorna seus dados completos com sucesso
    - Dado que informo um código numérico inexistente, então a aplicação rejeita a consulta e informa que a categoria não foi encontrada
    - Dado que informo uma descrição inexistente, então a aplicação rejeita a consulta e informa que a categoria não foi encontrada
```

```md
TC12 - Consulta de categoria por código numérico bem sucedida
User Story: US5 - Consulta de categoria por código ou descrição

Pré-condições
    - Já existe na base uma categoria cujo código numérico corresponde ao código da categoria a ser consultada

Entradas/Ações
    - Informar um código numérico
    - Acionar a consulta

Resultados Esperados
    - A aplicação retorna 1 categoria
    - São exibidos: código numérico, descrição e informações de auditoria (última alteração)

Pós-condições
    -
```

```md
TC13 - Consulta de categoria por código numérico rejeitada
User Story: US5 - Consulta de categoria por código ou descrição

Pré-condições
    - Não existe na base uma categoria cujo código numérico corresponde ao código da categoria a ser consultada

Entradas/Ações
    - Informar um código numérico
    - Acionar a consulta

Resultados Esperados
    - A aplicação rejeita a consulta e exibe mensagem informando que não existe uma categoria com o código numérico

Pós-condições
    -
```

```md
TC14 - Consulta de categoria por descrição bem sucedida
User Story: US5 - Consulta de categoria por código ou descrição

Pré-condições
    - Já existe na base uma categoria cuja descrição é idêntica à descrição da categoria a ser consultada

Entradas/Ações
    - Informar uma descrição válida (apenas letras, números e acentuação)
    - Acionar a consulta

Resultados Esperados
    - A aplicação retorna 1 categoria
    - São exibidos: código numérico, descrição e informações de auditoria (última alteração)

Pós-condições
    -
```

```md
TC15 - Consulta de categoria por descrição rejeitada
User Story: US5 - Consulta de categoria por código ou descrição

Pré-condições
    - Não existe na base uma categoria cuja descrição é idêntica à descrição da categoria a ser consultada

Entradas/Ações
    - Informar uma descrição válida (apenas letras, números e acentuação)
    - Acionar a consulta

Resultados Esperados
    - A aplicação rejeita a consulta e exibe mensagem informando que não existe uma categoria com a descrição

Pós-condições
    -
```

Após tudo implementado no servidor, valide os casos de teste através da tela de categorias no cliente, e, havendo alguma necessidade de adaptação no cliente, faça.

> Dica: Por questão de brevidade, se preocupe em implementar todas as funcionalidades primeiro, e só após começar os ajustes finos de validações, aplicação de outras regras, etc.
