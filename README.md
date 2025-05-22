# DotNet 9 e OpenTelemetry
Projeto de exemplo de uma Web API em DOTNET 9 e OpenTelemetry. A ideia principal foi utilizar alguns dos recursos mais importantes relacionados ao tema de Observabilidade.

Vale ressaltar que o projeto foi utilizado apenas para estudo e com a visão de um desenvolvedor. Há uma infinidade de conceitos e opções a respeito de Observabilidade e OpenTelemetry que podem e devem ser explorados além daqueles utilizados neste projeto.

Características do projeto:
* A aplicação (Web API) possui um endpoint para consulta de cidades por estado (UF) do Brasil. A aplicação Web está configurada para ser executada na porta 8080 (http://localhost:8080/scalar/v1).
* Todos os recursos necessários para execução da aplicação de exemplo e das ferramentas utilizadas estão definidos no arquivo `docker-compose.yaml`.
* A Web API realiza a consulta de cidades por UF em um banco SQL Server. O banco SQL Server é criado e preenchido ao subir os containers com docker compose.
* A instrumentação da aplicação foi realizada utilizando [OpenTelemetry](https://opentelemetry.io/) com [Collector](https://opentelemetry.io/docs/collector/). A utilização do Collector permite que se escolha ou se altere as ferramentas de métricas, tracing e logs sem a que seja necessário alterar o código da aplicação (*vendor agnostic*). Além disso, o Collector fornece uma variedade de outras vantagens, como por exemplo a possibilidade de processar ou alterar mensagens recebidas antes que as mesmas sejam enviadas aos [Exporters](https://opentelemetry.io/docs/collector/configuration/#exporters).
* O [Prometheus](https://prometheus.io/) foi utilizado como ferramenta para métricas e está configurado para rodar na porta 9090.
* O [Jaeger](https://www.jaegertracing.io/) foi utilizado como ferramenta para tracing. Para visualizar os dados no navegador, basta acessar o endereço http://localhost:16686/search.
* Os logs estão sendo gerados no console da aplicação Web.
* O Grafana foi utilizado para melhor visualizar os dados de métricas coletadas e está configurado para rodar na porta 3000. Para visualizar os dados, é necessário configurar no Grafana um data source para o Prometheus e logo após, criar os dashboards. Apenas como exemplo, há um dashboard publicado pelo time do .NET que pode ser utilizado (https://grafana.com/grafana/dashboards/19924-asp-net-core/).

#### Imagem da Web API em .NET 9 para consulta de cidades
![image](https://github.com/user-attachments/assets/bd4840d6-d6dd-4a20-8a86-acb1e6c2abaf)

#### Imagem do Prometheus com a métrica de consultas por UF
![image](https://github.com/user-attachments/assets/cc08bb65-98cd-46ed-b0f5-0c48899ce32d)

#### Imagens do Jager com tracing
![image](https://github.com/user-attachments/assets/f0054169-db24-4d29-94b9-34035bad9092)
![image](https://github.com/user-attachments/assets/1cf83416-25d2-46e1-9e27-fbc8857683ea)

#### Imagens do Grafana com dois Dashboards de exemplo
![image](https://github.com/user-attachments/assets/46bfc476-ca9f-4146-aee3-1ae6a166ec8a)
![image](https://github.com/user-attachments/assets/6e13d67a-b567-4e3a-9fdd-cb980dff8df7)

#### Referências
* [Blog do Tiago Tartari](https://tiagotartari.net/observabilidade-em-aplicacoes-dotnet-com-opentelemetry.html)
* [Docs do OpenTelemetry](https://opentelemetry.io/docs/languages/dotnet/getting-started/)
* [Docs Microsoft](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel)
* [Docs Prometheus](https://prometheus.io/docs/introduction/overview/)
* [Docs Grafana](https://grafana.com/docs/grafana/latest/)
* [Docs Jeager](https://www.jaegertracing.io/docs/2.6/getting-started/)
