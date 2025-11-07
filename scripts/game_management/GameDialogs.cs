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
    {"Loss_2", new Dictionary
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
    {"Loss_3", new Dictionary
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
    {"Win_1", new Dictionary
        { {0, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Se é o que deseja, espie." },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
            {1, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Através da porta." },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },//TODO! colocar imagem da porta em tela cheia
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
            {2, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Por de trás dessa porta, há o que era você." },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },//TODO! por imagem da fechadura em tela cheia
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
            {3, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Não se deixe levar por algo tão vago como uma vida." },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },//TODO! por imagem do homem, blurry, através da fechadura
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },
            {4, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "???" },
                    { "dialog", "Lembre-se de porque você está aqui." },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },//TODO! por imagem do homem de costas no limbo
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },

            {5, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "Introspecção" },
                    { "dialog", "Você tem a sensação de familiaridade, como ao ver uma foto de muito tempo atrás, tão antiga que nem se lembra mais quando foi tirada." },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },//TODO! por imagem do homem de costas no limbo
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },

            {6, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "" },
                    { "dialog", "Pela primeira vez desde que você chegou na sala, você consegue sentir suas mãos." },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" },
                            { "args", new Godot.Collections.Array()}
                        }
                    }
                }
            },

            { 7, new Dictionary {
                    { "icon", "res://icon.svg" },
                    { "title", "" },
                    { "dialog", "Elas querem [shake rate=20.0 level=5 connected=1][b]continuar jogando[/b][/shake]." },
                    { "method", new Dictionary
                        {
                            { "object", new GodotObject() },
                            { "methodPath", "" }, //TODO: REMOVER IMAGEM DA TELA
                            { "args", new Godot.Collections.Array()}
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
    {"Win_3", new Dictionary
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
