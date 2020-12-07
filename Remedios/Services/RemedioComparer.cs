using Remedios.Models;
using Remedios.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.Services
{
    public class RemedioComparer : IEqualityComparer<Remedio>
    {
        public bool Equals([AllowNull] Remedio x, [AllowNull] Remedio y)
        {
            if (long.Equals(x.Id, y.Id))
            {
                return true;
            }
            return false;
        }

        public int GetHashCode([DisallowNull] Remedio obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
