using NZWalks.API.Models.Domain;
using System.Net;

namespace NZWalks.API.Repositories
{
    public class IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
