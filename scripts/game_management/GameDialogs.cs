using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class GameDialogs : Resource
{

public static Dictionary DialogData = new Dictionary
{
    {"Loss_1", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method", default }
                }
            },
        }

    },
    {"Loss_2", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method", default
                    }
                }
            },
        }

    },
    {"Loss_3", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method",default}
                }
            },
        }

    },
    {"Win_1", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "..." },
                    { "method", default}
                }
            },
            {1, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Se é o que deseja, espie. Através da porta." },
                    { "method", Callable.From( () =>
                        FullScreenImage.Instance.FadeImage(
                            ResourceLoader.Load<Texture2D>("res://assets/cutscenes/door.jpg"),
                            1.5) 
                        )
                    }
                }
            },
            {2, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Por de trás dessa porta, há o que era você." },
                    { "method", Callable.From( () =>
                        FullScreenImage.Instance.ZoomImage(10, new Vector2I(91*5/4, 56*5/4), 5) 
                        )
                    }
                }
            },
            {3, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Não se deixe levar por algo tão vago como uma vida." },
                    { "method", default }
                }
            },
            {4, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Lembre-se de porque você está aqui." },

                    { "method", Callable.From( () => {
                        FullScreenImage.Instance.FadeImage(ResourceLoader.Load<Texture2D>("res://assets/cutscenes/man.jpg"), 1.5);
                        FullScreenImage.Instance.RemoveZoom(1.5);
                        })
                    }
                }
            },

            {5, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "" },
                    { "dialog", "Você tem a sensação de familiaridade, como ao ver uma foto de muito tempo atrás, tão antiga que nem se lembra mais quando foi tirada." },
                    { "method", Callable.From( () => FullScreenImage.Instance.FadeImage(null, 1.5) ) }
                }

            },

            {6, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "" },
                    { "dialog", "Pela primeira vez desde que você chegou na sala, você consegue sentir suas mãos." },
                    { "method", default }
                }
            },

            { 7, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "" },
                    { "dialog", "Elas querem [shake rate=20.0 level=5 connected=1][b]continuar jogando[/b][/shake]." },
                    { "method", new Dictionary
                        {
                            { "object", default },
                            { "methodPath", default },
                            { "args", default },
                        }
                    }
                }
            }
        }

    },
    {"Win_2", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method",default}
                }
            },
        }

    },
    {"Win_3", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "character name" },
                    { "dialog", "dialog" },
                    { "method",default}
                }
            },
        }

    }
};
}
