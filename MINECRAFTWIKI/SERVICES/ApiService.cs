using System;
using System.Collections.Generic;
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
            try
            {
                var response = await _httpClient.GetStringAsync(_apiUrl);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var entities = JsonSerializer.Deserialize<List<MinecraftEntity>>(response, options) ?? new List<MinecraftEntity>();

                // Reemplazamos las imágenes rotas con las de nuestro diccionario
                foreach (var entity in entities)
                {
                    if (_imageFixes.ContainsKey(entity.Name))
                    {
                        entity.Image = _imageFixes[entity.Name];
                    }
                }

                return entities;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al consumir la API: {ex.Message}");
                return new List<MinecraftEntity>();
            }
        }
    }
}
