using HeaderDetection.Models;

namespace HeaderDetection.Interfaces
{
    public interface IStorage
    {
        void InsertText(string text, int row, int column);
        void MergeRow(int row, int beginColumn, int endColumn);
        void MergeColumn(int column, int beginRow, int endRow);
    }
}