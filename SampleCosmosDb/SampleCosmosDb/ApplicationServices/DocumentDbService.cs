using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleCosmosDb.Constants;
using SampleCosmosDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleCosmosDb.ApplicationServices
{
    public class DocumentDbService
    {
        readonly string _databaseId = "SampleCosmos";
        string _collectionId;
        readonly DocumentClient _readonlyClient = new DocumentClient(new Uri(DocumentDbConstants.Url), DocumentDbConstants.ReadOnlyPrimaryKey);
        Uri _documentCollectionUri;

        int _networkIndicatorCount = 0;
     
        public DocumentDbService(string collectionId)
        {
            _collectionId = collectionId;
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, collectionId);
        }

        public Uri GetDocumentCollectionUri<T>(T document) where T : CosmosDbModel<T>
        {
            return UriFactory.CreateDocumentCollectionUri(_databaseId, document.CollectionId);
        }
        public async Task<List<T>> GetAll<T>() where T : CosmosDbModel<T>
        {
            SetActivityIndicatorStatus(true);

            try
            {
                return await Task.Run(() => _readonlyClient
                    .CreateDocumentQuery<T>(_documentCollectionUri)
                    .Where(x => x.TypeName.Equals(typeof(T).Name))?.ToList())
                    .ConfigureAwait(false);
            }
            finally
            {
                SetActivityIndicatorStatus(false);
            }
        }

        public async Task<T> Get<T>(string id)
        {
            SetActivityIndicatorStatus(true);

            try
            {

                var result = await _readonlyClient.ReadDocumentAsync<T>(CreateDocumentUri(id)).ConfigureAwait(false);

                if (result.StatusCode != HttpStatusCode.Created)
                    return default(T);

                return result;
            }
            finally
            {
                SetActivityIndicatorStatus(false);
            }
        }

        public async Task<Document> Update<T>(T document) where T : CosmosDbModel<T>
        {
            SetActivityIndicatorStatus(true);

            try
            {
                var documentClient = GetReadWriteDocumentClient();

                var doc = await documentClient?.ReplaceDocumentAsync(CreateDocumentUri(document.Id), document);
                return doc;
            }
            finally
            {
                SetActivityIndicatorStatus(false);
            }
        }

        public async Task<Document> Create<T>(T document) where T : CosmosDbModel<T>
        {
            SetActivityIndicatorStatus(true);

            try
            {
                var documentClient = GetReadWriteDocumentClient();

                return await documentClient?.CreateDocumentAsync(_documentCollectionUri, document);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                SetActivityIndicatorStatus(false);
            }
        }

        public async Task<HttpStatusCode> Delete(string id)
        {
            SetActivityIndicatorStatus(true);

            try
            {
                var readWriteClient = GetReadWriteDocumentClient();
                if (readWriteClient == null)
                    return default(HttpStatusCode);

                var result = await readWriteClient?.DeleteDocumentAsync(CreateDocumentUri(id));

                return result?.StatusCode ?? throw new HttpRequestException("Delete Failed");
            }
            finally
            {
                SetActivityIndicatorStatus(false);
            }

        }

        Uri CreateDocumentUri(string id) =>
            UriFactory.CreateDocumentUri(_databaseId, _collectionId, id);

        DocumentClient GetReadWriteDocumentClient()
        {
            var url = DocumentDbConstants.Url;
            var key = DocumentDbConstants.ReadWritePrimaryKey;

            return new DocumentClient(new Uri(url), key);
        }

        void SetActivityIndicatorStatus(bool isNetworkConnectionActive)
        {
            if (isNetworkConnectionActive)
            {
                _networkIndicatorCount++;
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = true);
            }
            else if (--_networkIndicatorCount <= 0)
            {
                _networkIndicatorCount = 0;
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = false);
            }
        }
    }
}
