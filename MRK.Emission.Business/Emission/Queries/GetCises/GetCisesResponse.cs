using MRK.Emission.Domain.Models;
using System.Collections.Generic;

namespace MRK.Emission.Business.Emission.Queries.GetCises
{
    public class GetCisesResponse
    {
        public List<CisInfo> CisList { get; set; }
    }
}