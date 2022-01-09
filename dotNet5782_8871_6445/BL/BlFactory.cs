using BlApi;

namespace BL
{
    public static class BlFactory
    {
       public static IBL GetBl()
        {
            return BlApi.BL.Instance;
        }
    }
}
