using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA2017Test
{
    public class MyRSAProvider:IDisposable
    {
        public MyRSAProvider()
        {
            
        }
        public class PublicKey
        {
            public PublicKey()
            {

            }

            public  string N { get; set; }
            public  string E { get; set; }
            public string Qinv { get; set; }
            public string Ep { get; set; }
            public string Eq { get; set; }
            public string P { get; set; }
            public string Q { get; set; }
            public virtual MyMyKeys MyMyKeys { get; set; }

        }
        public  class PrivateKey
        {
            public PrivateKey()
            {

            }

            public  string N { get; set; }
            public  string D { get; set; }
            public string Dp { get; set; }
            public string Dq { get; set; }
            public string Qinv { get; set; }
            public string P { get; set; }
            public string Q { get; set; }
            public virtual MyMyKeys MyMyKeys { get; set; }

        }

        public  class MyMyKeys
        {
            public  MyMyKeys()
            {
                this.PublicKey = new ObservableCollection<PublicKey>();
                this.PrivateKey = new ObservableCollection<PrivateKey>();
            }

            
            public int KeySize { get; set; }

            public virtual ObservableCollection<PublicKey> PublicKey { get; private set; }
            public virtual ObservableCollection<PrivateKey> PrivateKey { get; private set; }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MyRSAProvider() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
