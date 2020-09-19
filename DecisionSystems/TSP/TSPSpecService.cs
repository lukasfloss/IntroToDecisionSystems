using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace DecisionSystems.TSP
{

    public class TSPSpecService
    {
        public async Task<SerializableTSPSpec[]> GetSpecs()
        {
            var content = await File.ReadAllTextAsync(@"TSP\tspspecs.json");
            return JsonConvert.DeserializeObject<SerializableTSPSpec[]>(content);
        }
    }
}
