using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_Network
{
    public struct Network_Struct
    {
        public int InputLayers_Count;
        public int[] InputLayers_Sizes;
        public int HiddenLayers_Count;
        public int[] HiddenLayers_Sizes;
        public int OutputLayer_Size;

        public Network_Struct(int[] inputLayers_Sizes, int[] hidenLayers_Sizes, int outputLayer_Size)
        {
            InputLayers_Sizes = inputLayers_Sizes ?? throw new ArgumentNullException(nameof(inputLayers_Sizes));
            InputLayers_Count = inputLayers_Sizes.Length;

            HiddenLayers_Sizes = hidenLayers_Sizes ?? throw new ArgumentNullException(nameof(hidenLayers_Sizes));
            HiddenLayers_Count = hidenLayers_Sizes.Length;
            OutputLayer_Size = outputLayer_Size;
        }
    }
}
