using Adly.Application.Contracts.FileService.Models;

namespace Adly.Application.Contracts.FileService.Interface;

public interface IFileService
{
    Task<List<SaveFileModelResult>> SaveFilesAsync(List<SaveFileModel> files, CancellationToken cancellationToken = default);

    Task<GetFileModel[]> GetFilesByNameAsync(List<string> fileNames, CancellationToken cancellationToken = default);

    Task RemoveFilesAsync(List<string> fileNames, CancellationToken cancellationToken = default);
}