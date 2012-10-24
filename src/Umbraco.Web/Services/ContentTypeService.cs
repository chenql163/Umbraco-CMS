﻿using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Core.Persistence.Repositories;
using Umbraco.Core.Persistence.UnitOfWork;

namespace Umbraco.Web.Services
{
    public class ContentTypeService : IContentTypeService
    {
        private readonly IUnitOfWorkProvider _provider;
        private readonly IContentService _contentService;

        public ContentTypeService(IContentService contentService) : this(contentService, new PetaPocoUnitOfWorkProvider())
        {}

        public ContentTypeService(IContentService contentService, IUnitOfWorkProvider provider)
        {
            _contentService = contentService;
            _provider = provider;
        }

        /// <summary>
        /// Gets an <see cref="IContentType"/> object by its Id
        /// </summary>
        /// <param name="id">Id of the <see cref="IContentType"/> to retrieve</param>
        /// <returns><see cref="IContentType"/></returns>
        public IContentType GetContentType(int id)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IContentTypeRepository, IContentType, int>(unitOfWork);
            return repository.Get(id);
        }

        /// <summary>
        /// Gets an <see cref="IContentType"/> object by its Alias
        /// </summary>
        /// <param name="alias">Alias of the <see cref="IContentType"/> to retrieve</param>
        /// <returns><see cref="IContentType"/></returns>
        public IContentType GetContentType(string alias)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IContentTypeRepository, IContentType, int>(unitOfWork);

            var query = Query<IContentType>.Builder.Where(x => x.Alias == alias);
            var contentTypes = repository.GetByQuery(query);

            return contentTypes.FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of all available <see cref="IContentType"/> objects
        /// </summary>
        /// <param name="ids">Optional list of ids</param>
        /// <returns>An Enumerable list of <see cref="IContentType"/> objects</returns>
        public IEnumerable<IContentType> GetAllContentTypes(params int[] ids)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IContentTypeRepository, IContentType, int>(unitOfWork);
            return repository.GetAll(ids);
        }

        /// <summary>
        /// Gets a list of children for a <see cref="IContentType"/> object
        /// </summary>
        /// <param name="id">Id of the Parent</param>
        /// <returns>An Enumerable list of <see cref="IContentType"/> objects</returns>
        public IEnumerable<IContentType> GetContentTypeChildren(int id)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IContentTypeRepository, IContentType, int>(unitOfWork);

            var query = Query<IContentType>.Builder.Where(x => x.ParentId == id);
            var contentTypes = repository.GetByQuery(query);
            return contentTypes;
        }

        /// <summary>
        /// Saves a single <see cref="IContentType"/> object
        /// </summary>
        /// <param name="contentType"><see cref="IContentType"/> to save</param>
        public void Save(IContentType contentType)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IContentTypeRepository, IContentType, int>(unitOfWork);

            repository.AddOrUpdate(contentType);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Saves a collection of <see cref="IContentType"/> objects
        /// </summary>
        /// <param name="contentTypes">Collection of <see cref="IContentType"/> to save</param>
        public void Save(IEnumerable<IContentType> contentTypes)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IContentTypeRepository, IContentType, int>(unitOfWork);

            foreach (var contentType in contentTypes)
            {
                repository.AddOrUpdate(contentType);
            }
            unitOfWork.Commit();
        }

        /// <summary>
        /// Deletes a single <see cref="IContentType"/> object
        /// </summary>
        /// <param name="contentType"><see cref="IContentType"/> to delete</param>
        /// <remarks>Deleting a <see cref="IContentType"/> will delete all the <see cref="IContent"/> objects based on this <see cref="IContentType"/></remarks>
        public void Delete(IContentType contentType)
        {
            _contentService.DeleteContentOfType(contentType.Id);

            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IContentTypeRepository, IContentType, int>(unitOfWork);

            repository.Delete(contentType);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Deletes a collection of <see cref="IContentType"/> objects
        /// </summary>
        /// <param name="contentTypes">Collection of <see cref="IContentType"/> to delete</param>
        /// <remarks>Deleting a <see cref="IContentType"/> will delete all the <see cref="IContent"/> objects based on this <see cref="IContentType"/></remarks>
        public void Delete(IEnumerable<IContentType> contentTypes)
        {
            var contentTypeList = contentTypes.ToList();
            foreach (var contentType in contentTypeList)
            {
                _contentService.DeleteContentOfType(contentType.Id);
            }

            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IContentTypeRepository, IContentType, int>(unitOfWork);

            foreach (var contentType in contentTypeList)
            {
                repository.Delete(contentType);
            }
            unitOfWork.Commit();
        }

        /// <summary>
        /// Gets an <see cref="IMediaType"/> object by its Id
        /// </summary>
        /// <param name="id">Id of the <see cref="IMediaType"/> to retrieve</param>
        /// <returns><see cref="IMediaType"/></returns>
        public IMediaType GetMediaType(int id)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMediaTypeRepository, IMediaType, int>(unitOfWork);
            return repository.Get(id);
        }

        /// <summary>
        /// Gets an <see cref="IMediaType"/> object by its Alias
        /// </summary>
        /// <param name="alias">Alias of the <see cref="IMediaType"/> to retrieve</param>
        /// <returns><see cref="IMediaType"/></returns>
        public IMediaType GetMediaType(string alias)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMediaTypeRepository, IMediaType, int>(unitOfWork);

            var query = Query<IMediaType>.Builder.Where(x => x.Alias == alias);
            var contentTypes = repository.GetByQuery(query);

            return contentTypes.FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of all available <see cref="IMediaType"/> objects
        /// </summary>
        /// <param name="ids">Optional list of ids</param>
        /// <returns>An Enumerable list of <see cref="IMediaType"/> objects</returns>
        public IEnumerable<IMediaType> GetAllMediaTypes(params int[] ids)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMediaTypeRepository, IMediaType, int>(unitOfWork);
            return repository.GetAll(ids);
        }

        /// <summary>
        /// Gets a list of children for a <see cref="IMediaType"/> object
        /// </summary>
        /// <param name="id">Id of the Parent</param>
        /// <returns>An Enumerable list of <see cref="IMediaType"/> objects</returns>
        public IEnumerable<IMediaType> GetMediaTypeChildren(int id)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMediaTypeRepository, IMediaType, int>(unitOfWork);

            var query = Query<IMediaType>.Builder.Where(x => x.ParentId == id);
            var contentTypes = repository.GetByQuery(query);
            return contentTypes;
        }

        /// <summary>
        /// Saves a single <see cref="IMediaType"/> object
        /// </summary>
        /// <param name="mediaType"><see cref="IMediaType"/> to save</param>
        public void Save(IMediaType mediaType)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMediaTypeRepository, IMediaType, int>(unitOfWork);

            repository.AddOrUpdate(mediaType);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Saves a collection of <see cref="IMediaType"/> objects
        /// </summary>
        /// <param name="mediaTypes">Collection of <see cref="IMediaType"/> to save</param>
        public void Save(IEnumerable<IMediaType> mediaTypes)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMediaTypeRepository, IMediaType, int>(unitOfWork);

            foreach (var mediaType in mediaTypes)
            {
                repository.AddOrUpdate(mediaType);
            }
            unitOfWork.Commit();
        }

        /// <summary>
        /// Deletes a single <see cref="IMediaType"/> object
        /// </summary>
        /// <param name="mediaType"><see cref="IMediaType"/> to delete</param>
        /// <remarks>Deleting a <see cref="IMediaType"/> will delete all the <see cref="IMedia"/> objects based on this <see cref="IMediaType"/></remarks>
        public void Delete(IMediaType mediaType)
        {
            //TODO
            //_mediaService.DeleteMediaOfType(mediaType.Id);
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMediaTypeRepository, IMediaType, int>(unitOfWork);

            repository.Delete(mediaType);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Deletes a collection of <see cref="IMediaType"/> objects
        /// </summary>
        /// <param name="mediaTypes">Collection of <see cref="IMediaType"/> to delete</param>
        /// <remarks>Deleting a <see cref="IMediaType"/> will delete all the <see cref="IMedia"/> objects based on this <see cref="IMediaType"/></remarks>
        public void Delete(IEnumerable<IMediaType> mediaTypes)
        {
            //TODO
            /*
            var mediaTypeList = mediaTypes.ToList();
            foreach (var mediaType in mediaTypeList)
            {
                _mediaService.DeleteMediaOfType(mediaType.Id);
            }
             */

            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMediaTypeRepository, IMediaType, int>(unitOfWork);

            foreach (var mediaType in mediaTypes)
            {
                repository.Delete(mediaType);
            }
            unitOfWork.Commit();
        }
    }
}