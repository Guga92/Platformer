using System.Collections;
using System.Collections.Generic;

namespace Services.SceneLoading
{
    public interface ILoadingPipelineService
    {
        public void LoadGameScene(string name);
        
        public void LoadGameScene(int index);
    }
}