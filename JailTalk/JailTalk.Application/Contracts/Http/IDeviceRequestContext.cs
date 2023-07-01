using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailTalk.Application.Contracts.Http;

public interface IDeviceRequestContext
{
    Guid GetDeviceId();
    int GetJailId();
    Guid GetPrisonerId();
}
