Considerações sobre uso do OpenTelemetry para logs:
 Tem dois problemas para tentarmos utilizar o OpenTelemetry para envio de logs para o ELK:
 1. O exportador de logs do ELK está em beta: https://github.com/open-telemetry/opentelemetry-collector-contrib/blob/main/exporter/elasticsearchexporter/README.md
 2. O ELK parece estar em definição de recebimento de logs via Otel ainda. Ao ler as docs me parece que estão atuando em entender os traces como Log, assim como o Grafana Tempo envia para o Grafana Loki.
 Repositório: https://github.com/ty-elastic/otel-logging
 Artigo: https://www.elastic.co/observability-labs/blog/3-models-logging-opentelemetry

Considerações sobre o pacote Elastic.Serilog.Sinks
 O Elastic.Serilog.Sinkspacote foi projetado para funcionar com o Elasticsearch 8.x e superiores, não com o Elasticsearch versão 7.11.1. 
 Aqui está uma explicação mais detalhada:
 Compatibilidade:
 O Elastic.Serilog.Sinkspacote, que é o pacote recomendado para registro do Serilog no Elasticsearch, foi criado especificamente para o Elasticsearch 8.x e superior.
 Motivo da incompatibilidade:
 O processo de bootstrapping do pacote tenta carregar modelos projetados para o Elasticsearch 8.0 e posteriores, tornando-o incompatível com versões anteriores, como a 7.11.1. 
 Pacote legado:
 Se você estiver usando uma versão mais antiga do Elasticsearch, talvez seja necessário usar o Serilog.Sinks.Elasticsearchpacote legado, que não é mais mantido.
 Migração:
 Se estiver usando o pacote legado, você deverá migrar para o novo Elastic.Serilog.Sinkspacote quando estiver pronto para atualizar para o Elasticsearch 8.x ou superior.
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