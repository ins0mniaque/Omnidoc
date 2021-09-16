namespace Omnidoc
{
    public static class LevelOperator
    {
        public static bool   Is    ( this Levels levels, Levels other ) => ( levels & other ) != 0;
        public static Levels Match ( this Levels levels, Levels other ) => levels & other;
        public static Level  Top   ( this Levels levels ) => levels.Is ( Levels.Document  ) ? Level.Document  :
                                                             levels.Is ( Levels.Page      ) ? Level.Page      :
                                                             levels.Is ( Levels.Link      ) ? Level.Link      :
                                                             levels.Is ( Levels.Block     ) ? Level.Block     :
                                                             levels.Is ( Levels.Table     ) ? Level.Table     :
                                                             levels.Is ( Levels.Header    ) ? Level.Header    :
                                                             levels.Is ( Levels.Footer    ) ? Level.Footer    :
                                                             levels.Is ( Levels.Row       ) ? Level.Row       :
                                                             levels.Is ( Levels.Cell      ) ? Level.Cell      :
                                                             levels.Is ( Levels.Vertex    ) ? Level.Vertex    :
                                                             levels.Is ( Levels.Edge      ) ? Level.Edge      :
                                                             levels.Is ( Levels.Paragraph ) ? Level.Paragraph :
                                                             levels.Is ( Levels.Line      ) ? Level.Line      :
                                                             levels.Is ( Levels.Word      ) ? Level.Word      :
                                                                                              Level.None;
    }
}