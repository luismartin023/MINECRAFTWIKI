using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MINECRAFTWIKI.MODELS;

namespace MINECRAFTWIKI.SERVICES
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://paste.rs/neHSh";

        // Diccionario para reemplazar las imágenes que devuelven 404 por alternativas que sí cargan
        private readonly Dictionary<string, string> _imageFixes = new Dictionary<string, string>
        {
            { "Creeper", "https://mc-heads.net/avatar/MHF_Creeper/100" },
            { "Enderman", "https://mc-heads.net/avatar/MHF_Enderman/100" },
            { "Aldeano", "https://mc-heads.net/avatar/MHF_Villager/100" },
            { "Zombie", "https://mc-heads.net/avatar/MHF_Zombie/100" },
            { "Gato", "https://mc-heads.net/avatar/MHF_Ocelot/100" },
            { "Esqueleto", "https://mc-heads.net/avatar/MHF_Skeleton/100" },
            { "Araña", "https://mc-heads.net/avatar/MHF_Spider/100" },
            { "Escudo", "https://raw.githubusercontent.com/InventivetalentDev/minecraft-assets/1.19.4/assets/minecraft/textures/entity/shield_base.png" },
            { "Cama", "https://raw.githubusercontent.com/InventivetalentDev/minecraft-assets/1.11.2/assets/minecraft/textures/items/bed.png" }
        };

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<MinecraftEntity>> GetEntitiesAsync()
        {
            var entities = await LoadRemoteAsync();

            // Si el JSON remoto no responde o viene vacío, usamos la copia local
            if (entities.Count == 0)
            {
                entities = await LoadLocalAsync();
            }

            ApplyImageFixes(entities);
            return entities;
        }

        private async Task<List<MinecraftEntity>> LoadRemoteAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_apiUrl);
                return Deserialize(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al consumir la API: {ex.Message}");
                return new List<MinecraftEntity>();
            }
        }

        // Respaldo: el catálogo local se copia a la salida como API/MOBS.json
        private async Task<List<MinecraftEntity>> LoadLocalAsync()
        {
            try
            {
                var path = Path.Combine(AppContext.BaseDirectory, "API", "MOBS.json");
                return File.Exists(path)
                    ? Deserialize(await File.ReadAllTextAsync(path))
                    : new List<MinecraftEntity>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al leer el catálogo local: {ex.Message}");
                return new List<MinecraftEntity>();
            }
        }

        private static List<MinecraftEntity> Deserialize(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<MinecraftEntity>>(json, options) ?? new List<MinecraftEntity>();
        }

        // Reemplazamos las imágenes rotas con las de nuestro diccionario
        private void ApplyImageFixes(List<MinecraftEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (_imageFixes.ContainsKey(entity.Name))
                {
                    entity.Image = _imageFixes[entity.Name];
                }
            }
        }
    }
}
