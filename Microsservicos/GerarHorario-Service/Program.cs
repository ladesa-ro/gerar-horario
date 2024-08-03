using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.HorarioGerado;
using Sisgea.GerarHorario.Core;
using GerarHorario_Service.Config;
using System.Text.Json.Serialization;



var mock = @"{
  ""diaSemanaInicio"": 1,
  ""diaSemanaFim"": 5,
  ""turmas"": [
    {
      ""id"": ""1"",
      ""nome"": ""1A INFORMATICA"",
      ""diariosDaTurma"": [
        {
          ""id"": ""diario:1_3"",
          ""turmaId"": ""turma:1"",
          ""professorId"": ""1"",
          ""disciplinaId"": ""disciplina:3"",
          ""quantidadeMaximaSemana"": 1
        },
        {
          ""id"": ""diario:1_1"",
          ""turmaId"": ""turma:1"",
          ""professorId"": ""2"",
          ""disciplinaId"": ""disciplina:1"",
          ""quantidadeMaximaSemana"": 3
        },
        {
          ""id"": ""diario:1_2"",
          ""turmaId"": ""turma:1"",
          ""professorId"": ""1"",
          ""disciplinaId"": ""disciplina:2"",
          ""quantidadeMaximaSemana"": 2
        }
      ],
      ""disponibilidades"": [
        {
          ""diaSemanaIso"": 1,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
          
        },
        {
          ""diaSemanaIso"": 2,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
        },
        {
          ""diaSemanaIso"": 3,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
        },
        {
          ""diaSemanaIso"": 4,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }

          
        },
        {
          ""diaSemanaIso"": 5,
          ""intervalo"": 
            {
              ""horarioInicio"": ""13:00"",
              ""horarioFim"": ""17:29:59""
            }
        }
      ]
    },
    {
      ""id"": ""2"",
      ""nome"": ""1B INFORMATICA"",
      ""diariosDaTurma"": [
        {
          ""id"": ""diario:2_1"",
          ""turmaId"": ""turma:2"",
          ""professorId"": ""2"",
          ""disciplinaId"": ""disciplina:4"",
          ""quantidadeMaximaSemana"": 1
        },
        {
          ""id"": ""diario:2_3"",
          ""turmaId"": ""turma:2"",
          ""professorId"": ""1"",
          ""disciplinaId"": ""disciplina:1"",
          ""quantidadeMaximaSemana"": 3
        },
        {
          ""id"": ""diario:2_2"",
          ""turmaId"": ""turma:2"",
          ""professorId"": ""2"",
          ""disciplinaId"": ""disciplina:2"",
          ""quantidadeMaximaSemana"": 2
        }
      ],
      ""disponibilidades"": [
        {
          ""diaSemanaIso"": 1,
          ""intervalo"": 
            {
              ""horarioInicio"": ""13:00"",
              ""horarioFim"": ""17:29:59""
            }
        },
        {
          ""diaSemanaIso"": 2,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
        },
        {
          ""diaSemanaIso"": 3,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
        },
        {
          ""diaSemanaIso"": 4,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
        },
        {
          ""diaSemanaIso"": 5,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
        }
      ]
    },
    {
      ""id"": ""3"",
      ""nome"": ""1 PERIODO ADS"",
      ""diariosDaTurma"": [
        {
          ""id"": ""diario:3_1"",
          ""turmaId"": ""turma:3"",
          ""professorId"": ""2"",
          ""disciplinaId"": ""disciplina:4"",
          ""quantidadeMaximaSemana"": 1
        },
        {
          ""id"": ""diario:3_3"",
          ""turmaId"": ""turma:3"",
          ""professorId"": ""1"",
          ""disciplinaId"": ""disciplina:1"",
          ""quantidadeMaximaSemana"": 3
        },
        {
          ""id"": ""diario:3_2"",
          ""turmaId"": ""turma:3"",
          ""professorId"": ""2"",
          ""disciplinaId"": ""disciplina:2"",
          ""quantidadeMaximaSemana"": 2
        }
      ],
      ""disponibilidades"": [
        {
          ""diaSemanaIso"": 1,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
        },
        {
          ""diaSemanaIso"": 2,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
        },
        {
          ""diaSemanaIso"": 3,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
        },
        {
          ""diaSemanaIso"": 4,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
        },
        {
          ""diaSemanaIso"": 5,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
        }
      ]
    },
    {
      ""id"": ""4"",
      ""nome"": ""2 PERIODO ADS"",
      ""diariosDaTurma"": [
        {
          ""id"": ""diario:4_1"",
          ""turmaId"": ""turma:4"",
          ""professorId"": ""2"",
          ""disciplinaId"": ""disciplina:4"",
          ""quantidadeMaximaSemana"": 1
        },
        {
          ""id"": ""diario:4_3"",
          ""turmaId"": ""turma:4"",
          ""professorId"": ""1"",
          ""disciplinaId"": ""disciplina:1"",
          ""quantidadeMaximaSemana"": 3
        },
        {
          ""id"": ""diario:4_2"",
          ""turmaId"": ""turma:4"",
          ""professorId"": ""2"",
          ""disciplinaId"": ""disciplina:2"",
          ""quantidadeMaximaSemana"": 2
        }
      ],
      ""disponibilidades"": [
        {
          ""diaSemanaIso"": 1,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 2,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 3,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"":4,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 5,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        }
      ]
    }
  ],
  ""professores"": [
    {
      ""id"": ""1"",
      ""nome"": ""Flinstons"",
      ""disponibilidades"": [
        {
          ""diaSemanaIso"": 1,
          ""intervalo"": 
            {
              ""horarioInicio"": ""13:00"",
              ""horarioFim"": ""17:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 2,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""17:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 3,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
          
        },
        {
          ""diaSemanaIso"": 4,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
          
        },
        {
          ""diaSemanaIso"": 5,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
          
        },
        {
          ""diaSemanaIso"": 1,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 2,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 3,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 4,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 5,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        }
      ]
    },
    {
      ""id"": ""2"",
      ""nome"": ""Poucas"",
      ""disponibilidades"": [
        {
          ""diaSemanaIso"": 1,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""17:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 2,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""17:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 3,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""11:59:59""
            }
          
        },
        {
          ""diaSemanaIso"": 4,
          ""intervalo"": 
            {
              ""horarioInicio"": ""13:00"",
              ""horarioFim"": ""17:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 5,
          ""intervalo"": 
            {
              ""horarioInicio"": ""07:30"",
              ""horarioFim"": ""17:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 1,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 2,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 3,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        },
        {
          ""diaSemanaIso"": 4,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""22:39:59""
            }
          
        },
        {
          ""diaSemanaIso"": 5,
          ""intervalo"": 
            {
              ""horarioInicio"": ""19:00"",
              ""horarioFim"": ""23:29:59""
            }
          
        }
      ]
    }
  ],
  ""horariosDeAula"": [
    {
      ""horarioInicio"": ""07:30"",
      ""horarioFim"": ""08:19:59""
    },
    {
      ""horarioInicio"": ""08:20"",
      ""horarioFim"": ""09:09:59""
    },
    {
      ""horarioInicio"": ""09:10"",
      ""horarioFim"": ""09:59:59""
    },
    {
      ""horarioInicio"": ""10:20"",
      ""horarioFim"": ""11:09:59""
    },
    {
      ""horarioInicio"": ""11:10"",
      ""horarioFim"": ""11:59:59""
    },
    {
      ""horarioInicio"": ""13:00"",
      ""horarioFim"": ""13:49:59""
    },
    {
      ""horarioInicio"": ""13:50"",
      ""horarioFim"": ""14:39:59""
    },
    {
      ""horarioInicio"": ""14:40"",
      ""horarioFim"": ""15:29:59""
    },
    {
      ""horarioInicio"": ""15:50"",
      ""horarioFim"": ""16:39:59""
    },
    {
      ""horarioInicio"": ""16:40"",
      ""horarioFim"": ""17:29:59""
    },
    {
      ""horarioInicio"": ""19:00"",
      ""horarioFim"": ""19:49:59""
    },
    {
      ""horarioInicio"": ""19:50"",
      ""horarioFim"": ""20:39:59""
    },
    {
      ""horarioInicio"": ""20:40"",
      ""horarioFim"": ""21:29:59""
    },
    {
      ""horarioInicio"": ""21:50"",
      ""horarioFim"": ""22:39:59""
    },
    {
      ""horarioInicio"": ""22:40"",
      ""horarioFim"": ""23:29:59""
    }
  ],
  ""logDebug"": false
}";


var joptions = new JsonSerializerOptions
{
  PropertyNameCaseInsensitive = true,
  Converters = { new JsonStringEnumConverter() }
};

var options = JsonSerializer.Deserialize<GerarHorarioOptions>(mock, joptions);

Console.WriteLine(options.Professores[0].Nome);

var horario = Gerador.GerarHorario(options);


foreach (var hora in horario)
{
  var json = JsonSerializer.Serialize(hora);
  Console.WriteLine(json);
  break;
}
/*
#region Queue Tweaks


var factory = new ConnectionFactory()
{
    HostName = "message-broker",
    UserName = "admin",
    Password = "admin"
};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();
channel.QueueDeclare(
    queue: "gerar_horario",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

channel.QueueDeclare(
    queue: "horario_gerado",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

#endregion


Console.WriteLine(" [*] Waiting for messages.");

var serializationOptions = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true
};

var consumer = new EventingBasicConsumer(channel);
consumer.Received += ListenResponseInGerarHorario;

channel.BasicConsume(queue: "gerar_horario",
    autoAck: true,
    consumer: consumer);


#region Gerar Horario


IEnumerable<HorarioGerado> GerarHorario(GerarHorarioOptions options)
{
    return Gerador.GerarHorario(options);
}

void ListenResponseInGerarHorario(object? model, BasicDeliverEventArgs ea)
{
    try
    {
        if (ea == null)
        {
            Console.WriteLine("Erro: ea ou ea.Body é nulo.");
            return;
        }

        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("Êxito em receber a mensagem");

        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Erro: mensagem recebida é nula ou vazia.");
            return;
        }

        GerarHorarioOptions? gerarHorarioOptions = JsonSerializer.Deserialize<GerarHorarioOptions>(message, serializationOptions);
        if (gerarHorarioOptions == null)
        {
            Console.WriteLine("Erro: gerarHorarioOptions é nulo após a desserialização.");
            return;
        }
        Console.WriteLine("Êxito em desserializar");

        var horarioGerado = Gerador.GerarHorario(gerarHorarioOptions);
        if (horarioGerado == null)
        {
            Console.WriteLine("Erro: horarioGerado é nulo após a geração.");
            return;
        }
        Console.WriteLine("Êxito em gerar o horario");

        var horarioJson = JsonSerializer.Serialize(horarioGerado);
        if (string.IsNullOrWhiteSpace(horarioJson))
        {
            Console.WriteLine("Erro: horarioJson é nulo ou vazio após a serialização.");
            return;
        }
        Console.WriteLine("Êxito em serializar");

        PublishResponseIntoHorarioGerado(horarioJson);
    }
    catch (JsonException jsonEx)
    {
        Console.WriteLine($"Erro de JSON: {jsonEx.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}

void PublishResponseIntoHorarioGerado(string horarioGerado)
{
    var body = Encoding.UTF8.GetBytes(horarioGerado);

    channel.BasicPublish(exchange: string.Empty,
        routingKey: "horario_gerado",
        basicProperties: null,
        body: body);

    Console.WriteLine($" [x] PASSO 3, ENVIANDO SEGUNDA MENSAGEM {horarioGerado}");
}

Console.ReadLine();

#endregion

*/