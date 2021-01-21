using HeaderDetection.Models;

namespace HeaderDetection.Interfaces
{
    public interface IStorage
    {
        void InsertText(string text, int rowZeroBase, int columnZeroBase);
        void MergeRow(int rowZeroBase, int beginColumnZeroBase, int endColumnZeroBase);
        void MergeColumn(int columnZeroBase, int beginRowZeroBase, int endRowZeroBase);
    }
}