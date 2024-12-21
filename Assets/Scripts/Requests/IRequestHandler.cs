using System;

namespace Requests
{
    public interface IRequestHandler
    {
        public Type GetDataType();
    }
}