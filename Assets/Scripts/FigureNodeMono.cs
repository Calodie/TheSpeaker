using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace KeyboardMan2D
{
    /// <summary>
    /// 图的节点
    /// </summary>
    public class FigureNode
    {
        /// <summary>
        /// 图节点Mono
        /// </summary>
        public FigureNodeMono mono;

        /// <summary>
        /// 节点名
        /// </summary>
        public string nodeName;

        /// <summary>
        /// 通行通道
        /// </summary>
        public List<FigureNode> succNodes;

        /// <summary>
        /// 来向通道（仅用于广度优先遍历）
        /// </summary>
        public FigureNode prevNode;

        private List<List<FigureNode>> viewedLevels;

        public FigureNode(FigureNodeMono mono)
        {
            this.mono = mono;
            nodeName = mono.name;
            succNodes = new List<FigureNode>();
            prevNode = null;

            viewedLevels = null;
        }

        /// <summary>
        /// 构建图
        /// </summary>
        public void Build(List<FigureNode> existingNodes)
        {
            // 对于每一个monoSuccNode
            foreach (FigureNodeMono monoSuccNode in mono.succNodes)
            {
                // 检查这个monoSuccNode是否已存在于图中
                FigureNode builtNode = null;
                foreach (FigureNode existingNode in existingNodes)
                {
                    if (monoSuccNode == existingNode.mono)
                    {
                        builtNode = existingNode;
                    }
                }
                // 不存在，创建新节点
                if (builtNode == null)
                {
                    FigureNode succNode = new FigureNode(monoSuccNode);
                    succNodes.Add(succNode);

                    // 记录这个新节点
                    existingNodes.Add(succNode);

                    // 向下继续构造
                    succNode.Build(existingNodes);
                }
                // 存在，连接至旧节点
                else
                {
                    succNodes.Add(builtNode);
                }
            }
        }

        /// <summary>
        /// 广度优先遍历创建层级表
        /// </summary>
        public void BuildWidthLevel()
        {
            int level = 0;

            viewedLevels = new List<List<FigureNode>> { new List<FigureNode>() };
            viewedLevels[0].Add(this);

            while (true)
            {
                // 新建下一层
                viewedLevels.Add(new List<FigureNode>());

                // 对于当前层的所有节点
                foreach (FigureNode currentLevelNode in viewedLevels[level])
                {
                    // 对于节点的所有通道
                    foreach (FigureNode succNode in currentLevelNode.succNodes)
                    {
                        // 检查通道是否已存在于该层及更前层中
                        bool searched = false;
                        foreach (List<FigureNode> viewedLevel in viewedLevels)
                        {
                            if (viewedLevel.Contains(succNode))
                            {
                                searched = true;
                                break;
                            }
                        }
                        // 不存在，添加到下一层
                        if (!searched)
                        {
                            viewedLevels[level + 1].Add(succNode);
                            // 储存来向通道
                            succNode.prevNode = currentLevelNode;
                        }
                    }
                }

                // 若下一层已无任何节点，结束遍历
                if (viewedLevels[level + 1].Count <= 0)
                {
                    return;
                }

                // 切换至下一层
                level++;
            }
        }

        /// <summary>
        /// 广度优先查询，返回最短路径
        /// </summary>
        /// <returns></returns>
        public List<FigureNode> WidthSearch(string nodeName)
        {
            // 逐层查找
            foreach (List<FigureNode> viewedLevel in viewedLevels)
            {
                foreach (FigureNode node in viewedLevel)
                {
                    // 找到目标
                    if (nodeName == node.nodeName)
                    {
                        // 前向生成路径
                        List<FigureNode> route = new List<FigureNode> { node };
                        FigureNode routeNode = node.prevNode;
                        while (routeNode != null)
                        {
                            route.Add(routeNode);
                            routeNode = routeNode.prevNode;
                        }
                        // 反转路径
                        route.Reverse();
                        // 返回路径
                        return route;
                    }
                }
            }
            // 无目标，返回null
            return null;
        }
    }


    /// <summary>
    /// 图节点Mono
    /// </summary>
    public class FigureNodeMono : MonoBehaviour
    {
        /// <summary>
        /// 通行节点
        /// </summary>
        [Header("通行节点")]
        public FigureNodeMono[] succNodes;

        private FigureNode figure = null;

        /// <summary>
        /// 寻找通向一节点的最短路径
        /// </summary>
        /// <returns></returns>
        public List<FigureNode> FindRouteToNode(string targetNode)
        {
            if (figure == null)
            {
                figure = new FigureNode(this);

                // 构建图
                figure.Build(new List<FigureNode>() { figure });

                // 广度优先遍历创建层级表
                figure.BuildWidthLevel();
            }

            // 广度优先查询最短路径
            return figure.WidthSearch(targetNode);
        }
    }
}