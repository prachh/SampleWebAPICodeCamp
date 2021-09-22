using System;
using System.Threading.Tasks;
using SampleWebAPICodeCamp.Data;

namespace SampleWebAPICodeCamp.Repository.Provider
{
    public interface IIPRepository
    {
        Task<IPtoCountry> GetIpInfo(string ip);
    }
}
