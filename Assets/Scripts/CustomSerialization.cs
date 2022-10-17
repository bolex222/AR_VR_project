using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

public class CustomSerialization
{
    public int MyNumber = -1;
    public string MyString = string.Empty;

    /*public static byte[] Serialize(object obj)
    {
        CustomSerialization data = (CustomSerialization)obj;
        byte[] myNumberBytes = BitConverter.GetBytes(data.MyNumber);

        if (BitConverter.IsLittleEndian) { Array.Reverse(myNumberBytes); }


    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
