using AnyRestAPIMCPServer.Models;
using System.Text.Json;

namespace Utils
{
    internal static class OpenApiParser
    {
        public static OpenApiDocument Parse(string json)
        {
            using var doc = JsonDocument.Parse(json);

            // Extract first server url if present
            string? serverUrl = null;
            if (doc.RootElement.TryGetProperty("servers", out var serversEl) &&
                serversEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var s in serversEl.EnumerateArray())
                {
                    if (s.TryGetProperty("url", out var urlEl))
                    {
                        serverUrl = urlEl.GetString();
                        break;
                    }
                }
            }

            var ops = new List<OpenApiOperation>();
            if (doc.RootElement.TryGetProperty("paths", out var paths))
            {
                foreach (var pathProp in paths.EnumerateObject())
                {
                    var path = pathProp.Name;
                    foreach (var verbProp in pathProp.Value.EnumerateObject())
                    {
                        var httpMethod = verbProp.Name.ToUpperInvariant();
                        var opEl = verbProp.Value;
                        var opId = opEl.TryGetProperty("operationId", out var idEl) ? idEl.GetString() ?? "Operation" : "Operation";
                        var desc = opEl.TryGetProperty("description", out var dEl) ? dEl.GetString() ?? opId : opId;

                        var parameters = new List<OpenApiParameter>();
                        if (opEl.TryGetProperty("parameters", out var psEl))
                        {
                            foreach (var p in psEl.EnumerateArray())
                            {
                                var name = p.GetProperty("name").GetString() ?? "param";
                                var pDesc = p.TryGetProperty("description", out var pdEl) ? pdEl.GetString() ?? name : name;
                                var type = p.TryGetProperty("schema", out var sEl) && sEl.TryGetProperty("type", out var tEl)
                                    ? tEl.GetString() ?? "string" : "string";
                                var pIn = p.TryGetProperty("in", out var inEl) ? inEl.GetString() ?? "query" : "query";
                                var required = p.TryGetProperty("required", out var reqEl) && reqEl.ValueKind == JsonValueKind.True;
                                parameters.Add(new OpenApiParameter(name, pDesc, type, pIn, required));
                            }
                        }

                        var returnDesc = "Result";
                        if (opEl.TryGetProperty("responses", out var respEl) &&
                            respEl.TryGetProperty("200", out var okEl) &&
                            okEl.TryGetProperty("description", out var okDescEl))
                        {
                            returnDesc = okDescEl.GetString() ?? returnDesc;
                        }

                        ops.Add(new OpenApiOperation(opId, desc, path, httpMethod, parameters, returnDesc));
                    }
                }
            }

            return new OpenApiDocument(serverUrl, ops);
        }
    }
}