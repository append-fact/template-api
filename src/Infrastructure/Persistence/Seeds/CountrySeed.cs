using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Seeds
{
    public static class CountrySeed
    {
        public static async Task SeedCountryAsync(ApplicationDbContext context)
        {
            if (!context.Countries.Any())
            {
                context.Countries.AddRange(new List<Country>
                {
                    new Country
                    {
                        Name = "Argentina",
                        IsActive = true,
                        CreatedDate = new DateTime(), CreatedBy = "Append",
                    }
                });

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedProvincesAsync(ApplicationDbContext context)
        {
            var country = await context.Countries.FirstOrDefaultAsync(x => x.Name == "Argentina");

            if (!context.Provinces.Any() && country != null)
            {
                context.Provinces.AddRange(new List<Province>
        {
            new Province { Name = "Buenos Aires", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Catamarca", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Chaco", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Chubut", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Córdoba", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Corrientes", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Entre Ríos", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Formosa", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Jujuy", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "La Pampa", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "La Rioja", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Mendoza", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Misiones", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Neuquén", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Río Negro", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Salta", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "San Juan", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "San Luis", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Santa Cruz", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Santa Fe", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Santiago del Estero", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Tierra del Fuego", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Tucumán", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new Province { Name = "Ciudad Autónoma de Buenos Aires", CountryId = country.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" } // CABA
        });

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedCityAsync(ApplicationDbContext context)
        {
            var province = await context.Provinces.FirstOrDefaultAsync(x => x.Name == "Buenos Aires");
            if (!context.Cities.Any())
            {
                context.Cities.AddRange(new List<City>
        {
            // Primera sección: Partidos del Área Metropolitana de Buenos Aires (AMBA)
            new City { Name = "Almirante Brown", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Avellaneda", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Berazategui", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Esteban Echeverría", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Ezeiza", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Florencio Varela", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General San Martín", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Hurlingham", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Ituzaingó", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "José C. Paz", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "La Matanza", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Lanús", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Lomas de Zamora", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Malvinas Argentinas", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Merlo", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Moreno", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Morón", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Quilmes", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Fernando", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Isidro", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Miguel", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tigre", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tres de Febrero", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Vicente López", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },

            // Segunda sección: Interior de la provincia
            new City { Name = "25 de Mayo", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Adolfo Alsina", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Adolfo González Chaves", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Alberti", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Arrecifes", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Ayacucho", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Azul", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Bahía Blanca", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Balcarce", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Baradero", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Benito Juárez", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Berisso", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Bolívar", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Bragado", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Campana", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Cañuelas", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Carlos Casares", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Carlos Tejedor", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Carmen de Areco", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Castelli", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Chacabuco", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Chascomús", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Chivilcoy", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Colón", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Coronel Dorrego", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Coronel Pringles", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Coronel Rosales", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Coronel Suárez", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Daireaux", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Dolores", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Ensenada", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Escobar", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Exaltación de la Cruz", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Alvarado", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Arenales", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Belgrano", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Guido", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Juan Madariaga", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General La Madrid", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Las Heras", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Lavalle", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Paz", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Pinto", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Pueyrredón", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Rodríguez", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General San Martín", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Viamonte", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "General Villegas", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Guaminí", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Hipólito Yrigoyen", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Junín", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "La Costa", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Laprida", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Las Flores", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Leandro N. Alem", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Lincoln", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Lobería", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Lobos", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Luján", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Magdalena", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Maipú", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Mar Chiquita", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Marcos Paz", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Monte", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Monte Hermoso", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Navarro", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Necochea", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Patagones", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Pehuajó", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Pellegrini", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Pergamino", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Pila", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Pilar", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Pinamar", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Presidente Perón", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Puán", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Punta Indio", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Quilmes", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Ramallo", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Rauch", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Rivadavia", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Rojas", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Roque Pérez", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Saavedra", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Saladillo", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Salliqueló", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Salto", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Andrés de Giles", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Antonio de Areco", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Cayetano", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Fernando", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Isidro", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Miguel", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Nicolás", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "San Pedro", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Suipacha", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tandil", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tapalqué", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tigre", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tordillo", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tornquist", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Trenque Lauquen", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tres Arroyos", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tres de Febrero", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Tres Lomas", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Vicente López", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Villa Gesell", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Villarino", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" },
            new City { Name = "Zárate", ProvinceId = province.Id, CreatedDate = DateTime.UtcNow, CreatedBy = "Append" }
        });

                await context.SaveChangesAsync();
            }
        }


    }
}
