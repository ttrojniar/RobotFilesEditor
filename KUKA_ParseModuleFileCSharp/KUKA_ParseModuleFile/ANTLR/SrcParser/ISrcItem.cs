﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModuleFile.ANTLR.SrcParser
{
    public interface ISrcItem
    {
        int Line { get; set; }
        //void AddChild(ISrcParser item);
    }
}
