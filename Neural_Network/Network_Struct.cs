using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_Network
{
    public struct Network_Struct
    {
        public int InputLayer_Size;
        public int HiddenLayers_Count;
        public int[] HiddenLayers_Sizes;
        public int OutputLayer_Size;

        public Network_Struct(int inputLayer_Size, int[] hidenLayers_Sizes, int outputLayer_Size)
        {
            InputLayer_Size = inputLayer_Size;

            HiddenLayers_Sizes = hidenLayers_Sizes ?? throw new ArgumentNullException(nameof(hidenLayers_Sizes));
            HiddenLayers_Count = hidenLayers_Sizes.Length;
            OutputLayer_Size = outputLayer_Size;
        }
    }
}
