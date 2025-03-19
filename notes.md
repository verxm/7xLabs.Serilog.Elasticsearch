Considera��es sobre uso do OpenTelemetry para logs:
 Tem dois problemas para tentarmos utilizar o OpenTelemetry para envio de logs para o ELK:
 1. O exportador de logs do ELK est� em beta: https://github.com/open-telemetry/opentelemetry-collector-contrib/blob/main/exporter/elasticsearchexporter/README.md
 2. O ELK parece estar em defini��o de recebimento de logs via Otel ainda. Ao ler as docs me parece que est�o atuando em entender os traces como Log, assim como o Grafana Tempo envia para o Grafana Loki.
 Reposit�rio: https://github.com/ty-elastic/otel-logging
 Artigo: https://www.elastic.co/observability-labs/blog/3-models-logging-opentelemetry

Considera��es sobre o pacote Elastic.Serilog.Sinks
 O Elastic.Serilog.Sinkspacote foi projetado para funcionar com o Elasticsearch 8.x e superiores, n�o com o Elasticsearch vers�o 7.11.1. 
 Aqui est� uma explica��o mais detalhada:
 Compatibilidade:
 O Elastic.Serilog.Sinkspacote, que � o pacote recomendado para registro do Serilog no Elasticsearch, foi criado especificamente para o Elasticsearch 8.x e superior.
 Motivo da incompatibilidade:
 O processo de bootstrapping do pacote tenta carregar modelos projetados para o Elasticsearch 8.0 e posteriores, tornando-o incompat�vel com vers�es anteriores, como a 7.11.1. 
 Pacote legado:
 Se voc� estiver usando uma vers�o mais antiga do Elasticsearch, talvez seja necess�rio usar o Serilog.Sinks.Elasticsearchpacote legado, que n�o � mais mantido.
 Migra��o:
 Se estiver usando o pacote legado, voc� dever� migrar para o novo Elastic.Serilog.Sinkspacote quando estiver pronto para atualizar para o Elasticsearch 8.x ou superior.
 ```csharp
 private static LoggerConfiguration ConfigureElasticsearchkiSinkWithNewPackage(this LoggerConfiguration configuration)
 {
     return configuration
         .WriteTo.Console()
         .WriteTo.Elasticsearch(new[] { new Uri("http://localhost:9200") },
             options =>
             {
                 options.DataStream = new DataStreamName("serilog.elk.poc");
                 options.TextFormatting = new EcsTextFormatterConfiguration();
                 options.BootstrapMethod = BootstrapMethod.Failure;
                 options.ConfigureChannel = channelOptions =>
                 {
                     channelOptions.BufferOptions = new BufferOptions();
                 };
             });
 }
```