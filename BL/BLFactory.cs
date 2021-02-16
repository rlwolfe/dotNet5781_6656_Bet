using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLApi
{
    public static class BlFactory
    {
        public static IBL GetBL()
        {
            return BL.BLIMP.Instance;
        }
    }
}
