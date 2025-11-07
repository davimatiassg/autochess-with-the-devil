using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class GameDialogs : Resource
{
[Export]
public Dictionary DialogData = new Dictionary
{
    {"First Defeat", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
        }

    },
    {"Second Defeat", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
        }

    },
    {"Third Defeat", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
        }

    },
    {"First Victory", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
        }

    },
    {"Second Victory", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
        }

    },
    {"Third Victory", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
        }

    }
};
}
