using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IPasswordGenerator {
    string Generate(int length = 12);
}
