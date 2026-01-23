# Models, DTOs e Wrappers

Models, DTOs e Wrappers são três padrões essenciais para organizar e estruturar dados dentro do ecossistema do Banrisul (neste caso no MM4). Eles formam a base conceitual necessária para entender como os frameworks trabalham com objetos de informação em todas as suas camadas.

Antes de falarmos especificamente sobre o [TO](/dicionario-banrisul.md#to---transfer-object) — o principal tipo de objeto utilizado para representar dados de negócio e trafegar entre camadas dentro do MM4 — é importante compreendermos esses três padrões, pois eles são a fundação sobre a qual o TO se apoia.

## Model

O padrão **Model** é amplamente utilizado na engenharia de software, mas seu significado pode variar levemente conforme arquitetura ou abordagem adotadas. No contexto do MM4, trabalhamos com um entendimento mais voltado para um **objeto/estrutura de dados que _espelha_ a forma como as informações são armazenadas no base — ou seja — tabelas, colunas, tipos, chaves e relacionamentos.**

Em outras palavras, uma model representa o "formato persistido" dos dados. Ela não define regras de negócio, não carrega comportamentos salientes e não existe para trafegar entre camadas a priori. **Seu papel é estrutural — ela é ponto de referência, a versão "bruta", estável e persistida da informação**.

De forma simplória, uma base de dados contendo:

```sql
SELECT
    ID              -- INTEGER      NN (IDENTITY)
  , NOME            -- VARCHAR(100) NN
  , IDADE           -- INTEGER
  , EMAIL           -- VARCHAR(120) NN
  , SENHA_HASH      -- VARCHAR(200) NN
  , DATA_CADASTRO   -- TIMESTAMP    NN (DEFAULT CURRENT_TIMESTAMP)
FROM USUARIO;
```

Teria, na aplicação, uma model semelhante a esta:

```csharp
public class UsuarioModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
    public DateTime DataCadastro { get; set; }
}
```

## DTO

O padrão **DTO (Data Transfer Object)** existe para resolver exatamente a limitação conceitual da model: **trafegar dados entre camadas, módulos, sistemas e fronteiras de comunicação**.

Enquanto a Model representa o dado bruto, no "formato persistido" dos dados, o **DTO representa o dado abstraído, "em trânsito"**, moldado para a necessidade de uma operação específica — seja uma requisição HTTP, uma resposta de serviço, uma chamada entre módulos ou até a comunicação com sistemas externos.

Um DTO **não precisa (e muitas vezes não deve)** refletir exatamente a estrutura da tabela. Ele existe para **transportar apenas o que importa no contexto daquela operação**, garantindo isolamento entre camadas e evitando expor detalhes internos da persistência.

Imagine a mesma model de usuário:

```csharp
public class UsuarioModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
    public DateTime DataCadastro { get; set; }
}
```

Mas que em dado momento, para retornar informações para um cliente, não deve transportar junto `Id`, `SenhaHash` e `DataCadastro`. Para essa finalidade poderia ser construído um DTO condizente com a necessidade:

```csharp
public class UsuarioDto
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
}
```

## Wrapper

O termo **Wrapper** é um dos mais genéricos dentro da engenharia de software. No sentido amplo, um wrapper é simplesmente **algo que "embrulha", "envolve" ou "encapsula" outra coisa** — seja um valor primitivo, um objeto complexo, uma chamada de API, um fluxo de dados, um resultado de operação etc.

Linguagens possuem wrappers nativos (como `Nullable<T>` e `Task<T>`, por exemplo), frameworks criam wrappers utilitários, e arquiteturas utilizam wrappers para padronizar respostas, capturar metadados, validar estruturas, adicionar contexto, entre diversas outras finalidades.

Em outras palavras: **Wrappers não têm um formato único, nem um propósito único. Eles existem para organizar, complementar e contextualizar algo que já existe.**

### Wrappers no contexto de dados e transporte

Trazendo o conceito para mais próximo dos padrões discutidos (Model e DTO), também do próprio MM4, podemos sub-categorizar os wrappers como wrappers de dados e wrappers de transporte:

Um **Wrapper de dados** é um objeto criado para **agregar informações adicionais ou [Metadado](/dicionario-banrisul.md#metadado)s** que o dado em si não carrega em sua forma "bruta" — espelhada da base de dados, tais como:

- Status de campos
- Cálculos entre campos
- Paginações  
- Indicadores de processamento
- Status de validade do objeto em si

Uma representação de um wrapper de dados para a mesma model de usuário:

```csharp
public class UsuarioModel
{
    public int Id { get; set; }
    public string Nome { get; set; } // Pode estar vazio
    public int Idade { get; set; } // Pode estar zerada
    public string Email { get; set; } // Pode ser inválido
    public string SenhaHash { get; set; } // Pode não ter sido gerada ainda
    public DateTime DataCadastro { get; set; }
}
```

Agora com um wrapper que mantém um status de validade do objeto como um todo, por exemplo, poderia ser:

```csharp
public class UsuarioModelWrapper
{
    public UsuarioModel Usuario { get; set; }
    
    public bool IsUsuarioValido => IsNomePreenchido && IsIdadeValida && IsEmailValido && IsSenhaValida;

    public bool IsNomePreenchido => !string.IsNullOrWhiteSpace(Usuario.Nome);
    
    public bool IsIdadeValida => Usuario.Idade > 0;

    public bool IsEmailPreenchido => !string.IsNullOrWhiteSpace(Usuario.Email);

    public bool IsEmailValido => IsEmailPreenchido && ValidadorEmail.IsEmailValido(Usuario.Email);

    public bool IsSenhaValida => !string.IsNullOrWhiteSpace(Usuario.SenhaHash);
}
```

Já um **Wrapper de transporte** é utilizado quando precisamos **padronizar a forma como algo trafega** entre módulos, camadas ou serviços.  
Ele garante que **todas as operações sigam um mesmo formato**, independentemente do conteúdo interno.

Exemplos comuns:

- Respostas padronizadas para APIs
- Estruturas que sempre carregam seções como cabeçalhos e rodapés
- Objetos que transportam contexto + dados
- Envelopes de envio e recebimento entre serviços internos

Uma representação de um wrapper de transporte para o mesmo DTO de usuário:

```csharp
public class UsuarioDto
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
}
```

Agora com um wrapper de respostas de APIs, que contém além do próprio DTO alguns dados adicionais de uma resposta padrão para API:

```csharp
public class RespostaApi<UsuarioDto>
{
    public UsuarioDto Usuario { get; set; }
    
    public string IdSessao { get; set; }
    public int StatusCode { get; set; }
    public string Mensagem { get; set; }
}
```

Wrappers não substituem nem models nem DTOs. Eles **os organizam**, dão **contexto**, adicionam **contratos** e fazem com que o sistema inteiro fale a mesma "língua" quanto a expressão e transporte de dados.

## Laboratório

Neste laboratório, importe novamente o _boilerplate_ de **solução Cliente-Servidor - v3** fornecido na pasta desta aula. Você perceberá que o "framework" deste evoluiu novamente e de forma muito significativa. Ele passou agora a suportar múltiplas rotas para envios de requisições, operações com parâmetros de rota (mandar um id em um GET), abstraiu mais complexidades que eram desnecessárias dentro das execuções principais de cliente e servidor, fez correções importantes de problemas de _serialização_ e _desserialização_ JSON e também passou a contar com um mecanismo de persistência, que tem a capacidade de armazenar dados em uma pasta `data` abaixo da pasta raíz da solution, em arquivos também no formato `.json` — cada arquivo JSON é nomeado de acordo com configuração expressada no respectivo [DAO](../../dicionario-banrisul.md#dao---data-access-object). Os arquivos contém a lista de todos os dados persistidos para aquela model. Também existe um wrapper padronizador de respostas do servidor ao cliente, que encapsula os DTOs e agrega informações adicionais como status, mensagem personalizada, data e hora da execução, etc.
<!-- Boilerplate em [./_assets/01-cliente-servidor-boilerplate-v3/] -->

> Atenção: Esse framework é extremamente instável e frágil — possui uma série de lacunas de funcionalidades, tratamentos de erros e fluxos alternativos. Ele serve para fins didáticos, e sua utilização em ambientes produtivos é extremamente desencorajada.

Neste laboratório vamos utilizar a implementação do recurso `exemplos` como referência (dado que já está 100% implementado com todos os verbos e operações necessárias) para fazermos o mesmo para o nosso recurso de `idiomas`, utilizando as user stories criadas no laboratório de negócio e requisitos.

Para fins de brevidade, vamos fazer uma implementação extremamente básica em cima das stories, possivelmente ignorando algumas regras de negócio, validações, tratamentos de erros e fluxos alternativos.

### Passo 1: Modelagem dos dados pela equipe Bancos de Dados

Em uma etapa anterior, o time de desenvolvimento, em conjunto com a equipe [Bancos de Dados](../../dicionario-banrisul.md#equipe-bancos-de-dados) já efetuou a modelagem dos dados e criação das estruturas de dados com base nas stories e casos de testes criados. Vamos considerar essa etapa finalizada com a seguinte estrutura criada:

```sql
SELECT
    ID                            -- INTEGER      NN PK
  , DESCRICAO                     -- VARCHAR(100) NN
  , USUARIO_ULTIMA_ALTERACAO      -- VARCHAR(100)
  , DATA_HORA_ULTIMA_ALTERACAO    -- TIMESTAMP
FROM IDIOMA;
```

### Passo 2: Criar a model `IdiomaModel` e o DTO `IdiomaDto`

Após a etapa de modelagem e já com a estrutura em mãos, vamos criar a model de idiomas na pasta **Models** do projeto **Servidor**:

```csharp
using System;

namespace Servidor.Models
{
    public class IdiomaModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime DataHoraUltimaAlteracao { get; set; }
    }
}
```

Vamos criar o DTO de idioma na pasta **DTOs** do projeto **Servidor** — no projeto **Cliente** já está tudo pronto (vamos focar no servidor):

```csharp
using System;

namespace Servidor.DTOs
{
    public class IdiomaDto
    {
        public int Id { get; set; }
        public string CodigoIsoCombinado { get; set; }
        public string Descricao { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime DataHoraUltimaAlteracao { get; set; }
    }
}
```

> Nota: Ao contrário do que foi feito para `exemplos`, para `idiomas` vamos manter **um só DTO para entrada e saída**. Ambas as práticas são comuns, mas manter um só DTO é mais prático.

### Passo 3: Criar o mecanismo de persistência de idiomas

Na pasta **DAOs** do projeto **Servidor**, vamos adicionar uma classe `IdiomaDao`:

```csharp
using PseudoFramework.ServidorUtils;
using Servidor.Models;
using System;
using System.IO;

namespace Servidor.DAOs
{
    public class IdiomaDao: ObjetoPersistenciaJson<IdiomaModel>
    {
        // Nome do arquivo que será criado/mantido
        private const string ARQUIVO = "idiomas.json";

        // Pasta onde o arquivo ficará armazenado — Toda essa lógica aqui serve pra navegar até a raíz da solution
        private static string RaizProjeto = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "data"); // Alterar conforme conveniência

        // Construtor repassando para a base (para o framework) o endereço da pasta e o nome do arquivo
        public IdiomaDao() : base(Path.Combine(RaizProjeto, ARQUIVO)) { }
    }
}
```

Essa classe basicamente reusa todas as funcionalidades providas pelo framework através da classe `ObjetoPersistenciaJson<T>`. Ou seja, toda a lógica de como gerar arquivos JSON e lidar com os dados dentro deles, está **abstraída dentro do framework**.

### Passo 4: Criar a rota de idiomas

Agora que temos toda a base estrutural, vamos adaptar o hook do servidor para suportar também uma rota de idiomas, e vamos implementar as funcionalidades envolvidas

No método `RotearRequisicao(...)` da `Program.cs` do projeto **Servidor**, vamos adicionar a segunda opção de roteamento no switch:

```csharp
switch (recurso)
{
    // [...]

    case IdiomaRota.RECURSO:
        {
            rota = new IdiomaRota();

            break;
        }

    // [...]
}
```

A classe `IdiomaRota` ainda não existe — será nela que vamos implementar as funcionalidades. Vamos criá-la.

Recapitulando as user stories em seus formatos resumidos:

```gherkin
US1 - Inclusão de novo idioma

Como um analista de operações globais
Eu quero incluir um novo idioma informando seu código textual ISO combinado e sua descrição
Para que o banco possa registrar oficialmente os idiomas suportados nas operações internacionais
```

```gherkin
US2 - Remoção de idioma

Como um analista de operações globais
Eu quero remover um idioma previamente cadastrado
Para evitar que equipes selecionem idiomas obsoletos e reduzir erros operacionais
```

```gherkin
US3 - Alteração de dados de idioma

Como um analista de operações globais
Eu quero alterar a descrição de um idioma já cadastrado
Para corrigir descrições incorretas ou melhorar sua clareza
```

```gherkin
US4 - Listagem de idiomas

Como um gerente de expansão global
Eu quero visualizar a lista de todos os idiomas cadastrados
Para verificar se o banco já possui suporte linguístico para regiões com as quais estamos negociando
```

```gherkin
US5 - Consulta de idioma por código

Como um gerente de produtos digitais
Eu quero consultar um idioma pelo seu código
Para validar rapidamente operações com parceiros estrangeiros
```

vamos gerar a classe para atender às necessidades:

```csharp
using PseudoFramework.SharedUtils;
using Servidor.DAOs;
using Servidor.DTOs;
using Servidor.Models;
using Servidor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.Rotas
{
    public class IdiomaRota: IRota
    {
        public const string RECURSO = "idiomas";

        private static readonly IdiomaDao _dao = new IdiomaDao();

        public RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json)
        {
            try
            {
                switch (verboHttp)
                {
                    case "GET":
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                                return GetIdioma(id); // US5

                            return GetIdiomas(); // US4
                        }
                    case "POST":
                        return PostIdioma(json); // US1

                    case "PUT":
                        return PutIdioma(id, json); // US3

                    case "DELETE":
                        return DeleteIdioma(id); // US2

                    default:
                        return RespostaWrapper.EnveloparInsucesso("Operação de idioma não suportada.");
                }
            }
            catch (Exception exception)
            {
                return RespostaWrapper.EnveloparErro(exception);
            }
        }

        private static RespostaWrapper<IEnumerable<IdiomaDto>> GetIdiomas()
        {
            var listaIdiomasDto = _dao
                .Listar()
                .Select(idiomaModel => new IdiomaDto
                {
                    Id = idiomaModel.Id,
                    CodigoIsoCombinado = Feconid.CodigoToIso(idiomaModel.Id),
                    Descricao = idiomaModel.Descricao
                });

            if (!listaIdiomasDto.Any())
                return RespostaWrapper.EnveloparInsucesso(listaIdiomasDto, "Lista de idiomas está vazia.");

            return RespostaWrapper.EnveloparSucesso(listaIdiomasDto);
        }

        private static RespostaWrapper<IdiomaDto> GetIdioma(string id)
        {
            var idiomaModelUnico = _dao
                .Listar()
                .FirstOrDefault(idiomaModel => idiomaModel.Id == Convert.ToInt32(id));

            if (idiomaModelUnico == null)
                return RespostaWrapper.EnveloparInsucesso<IdiomaDto>(null, "Idioma não encontrado.");

            var idiomaDtoConsultado = new IdiomaDto
            {
                Id = idiomaModelUnico.Id,
                CodigoIsoCombinado = Feconid.CodigoToIso(idiomaModelUnico.Id),
                Descricao = idiomaModelUnico.Descricao,
                UsuarioUltimaAlteracao = idiomaModelUnico.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = idiomaModelUnico.DataHoraUltimaAlteracao
            };

            return RespostaWrapper.EnveloparSucesso(idiomaDtoConsultado);
        }

        private static RespostaWrapper<IdiomaDto> PostIdioma(string json)
        {
            var listaIdiomasModel = _dao.Listar();

            var idiomaDtoRecebido = ConversorJson.Desserializar<IdiomaDto>(json);

            var codigo = Feconid.IsoToCodigo(idiomaDtoRecebido.CodigoIsoCombinado);

            if (listaIdiomasModel.Any(idiomaModel => idiomaModel.Id == codigo))
                return RespostaWrapper.EnveloparInsucesso<IdiomaDto>(null, "Idioma já existe.");

            var idiomaModelIncluir = new IdiomaModel
            {
                Id = codigo,
                Descricao = idiomaDtoRecebido.Descricao
            };

            var idiomaModelIncluido = _dao.Incluir(idiomaModelIncluir);

            var idiomaDtoIncluido = new IdiomaDto
            {
                Id = idiomaModelIncluido.Id,
                CodigoIsoCombinado = Feconid.CodigoToIso(idiomaModelIncluido.Id),
                Descricao = idiomaModelIncluido.Descricao,
                UsuarioUltimaAlteracao = idiomaModelIncluido.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = idiomaModelIncluido.DataHoraUltimaAlteracao
            };

            return RespostaWrapper.EnveloparSucesso(idiomaDtoIncluido);
        }

        private static RespostaWrapper PutIdioma(string id, string json)
        {
            var idiomaDtoRecebido = ConversorJson.Desserializar<IdiomaDto>(json);

            var idiomaModelAlterar = new IdiomaModel
            {
                Descricao = idiomaDtoRecebido.Descricao,
                UsuarioUltimaAlteracao = idiomaDtoRecebido.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = DateTime.Now
            };

            var resultado = _dao.Alterar(idiomaModel => idiomaModel.Id == Convert.ToInt32(id), idiomaModelAlterar);

            if (!resultado)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }

        private static RespostaWrapper DeleteIdioma(string id)
        {
            var listaIdiomasModel = _dao.Listar();

            var codigo = Convert.ToInt32(id);

            if (!listaIdiomasModel.Any(idiomaModel => idiomaModel.Id == codigo))
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrado um idioma com o código informado.");

            var resultado = _dao.Remover(idiomaModel => idiomaModel.Id == codigo);

            if (!resultado)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }
    }
}
```

> Notas:
>
> - Nas user stories e casos de teste, existem uma série de regras e validações que nós acabamos desconsiderando aqui por questão de brevidade. Em cenários de mundo real a implementação precisa seguir em detalhe todas as especificações.
>
> - Observe que uma classe `Feconid` está sendo utilizada para efetuar as conversões. Essa classe utilitária está localizada no projeto **Servidor**, ou seja, nesse caso não faz parte do framework de fato, pois é uma classe utilitária que serve somente **ao domínio de idiomas** (separação de responsabilidades).

Execute as duas aplicações e opere a rota de idiomas via aplicação cliente.

## [Exercícios](02-exercicios.md)
