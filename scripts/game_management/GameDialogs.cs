using Godot;
using System;
using Godot.Collections;
using System.Threading.Tasks;

[GlobalClass]
public partial class GameDialogs : Resource
{

public static Dictionary DialogData = new Dictionary
{
    {"Pre_Menu", new Array<Dictionary> {
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Israel, não temas, pois as graças serão entregues perante você." },
            { "method", Callable.From( () => FullScreenImage.Instance.ResetImage() ) }
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Seu corpo parece estar falhando. Você tenta se lembrar de como chegou nesse lugar, e nada vem a sua mente." },
            { "method", default }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Não há porque temer, não há mais nada. Pode ser incômodo não lembrar-se de seu passado, mas digo-te, que é para teu melhor." },
            { "method", default }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Você tem duas opções. Existe apenas uma resposta correta. Deixe-me guiar-lhe para o além... ou jogue." },
            { "method", default }//TODO! abrir menu principal
        }
    }},

    {"Game_Start", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/idle.png" },
            { "title", "Israel" },
            { "dialog", "Vamos jogar." },
            { "method", Callable.From( () => FullScreenImage.Instance.ResetImage() ) }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "..." },
            { "method", default }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Uma lástima." },
            { "method", Callable.From( () => Lightworks.InitializationSequence() ) }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Pois bem, tome suas cartas e jogue. Espero que este passa-tempo lhe traga algum conforto." },
            { "method", Callable.From(()=>Tabletop.Instance.AnimateBoardTransition()) }

        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Uma carta por turno. Criaturas se movem sozinhas. Leve uma criatura até o meu lado do tabuleiro e você vencerá." },
            { "method", default }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Sua vez." },
            { "method", Callable.From(() => {
                TurnState.IsRoundRunning = true;
                _ = TurnState.LoopTurns(); })
            }
        },

    }},

    { "Loss_1", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eyes.png" },
            { "title", "???" },
            { "dialog", "Eu venci." },
            { "method", Callable.From( () => FullScreenImage.Instance.ResetImage() ) }
        },

        new Dictionary {
            { "icon", "res://assets/sprites/portraits/idle.png" },
            { "title", "Israel" },
            { "dialog", "(Algo sumiu. Não consigo discernir exatamente o quê, mas algo sumiu.)" },
            { "method", default }
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Algo que você nunca mais terá de volta. A falta disso te consome." },
            { "method", default }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/idle.png" },
            { "title", "Israel" },
            { "dialog", "(É como se eu tivesse esquecido de uma memória de infância preciosa, com um amigo muito precioso. Espera... Amigo?)" },
            { "method", default }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Não existe mais ninguém, mais nada, além do agora. Tu que decidistes pelo caminho mais longo." },
            { "method", default }
        }
    }},
    {"Loss_2", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Você se desfaz mais um pouco" },
            { "method", Callable.From( () => FullScreenImage.Instance.ResetImage() ) }
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Mesmo sem face, você consegue sentir que, aquilo está sorrindo para ti. Todos seus infinitos olhos, que vão muito mais a fundo que está sala consegue ser." },
            { "method", default }
        },
    
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/tense.png" },
            { "title", "Israel" },
            { "dialog", "Esta sala… eu nunca entrei nela. Eu só apareci aqui. " },
            { "method", default }
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "E vai desaparecer nela, logo logo, você sente." },
            { "method", default }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/sad.png" },
            { "title", "Israel" },
            { "dialog", "Eu, sinto? Não... não mais..." },
            { "method", default }
        },
    }},
    {"Loss_3", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "O anjo" },
            { "dialog", "Esta foi a derradeira vez." },
            { "method", Callable.From( () => {
                    FullScreenImage.Instance.ResetImage();
                    GameManager.GameEnd(1);
                })
            }
        }
    }},
    {"End_1", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "O anjo" },
            { "dialog", "Venha, ovelha perdida." },
            { "method", Callable.From( () => {
                    FullScreenImage.Instance.ResetImage();
                    FullScreenImage.Instance.FadeImage( ResourceLoader.Load<Texture2D>("res://assets/cutscenes/angel.jpg"), 1.5);
                    
                })
            }
        },

        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Tudo fica para trás. Seu passado. Suas memórias. Tudo que você poderia ter sido. Seus sonhos. Nada mais disso importa. Seu fim chegou." },
            { "method", default}
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Você acompanha O Anjo. Até a luz." },
            { "method", Callable.From( () =>
                {
                    FullScreenImage.Instance.FadeColor(Colors.White, 1.5);
                    Task.Delay(3000).ContinueWith((Task t) => GameManager.Credits());
                 }) }
        }
    }},
    {"Win_1", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "..." },
            { "method", Callable.From( () => FullScreenImage.Instance.ResetImage() ) }
        },

        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Se é o que deseja, espie... Através da porta..." },
            { "method", Callable.From( () =>
                FullScreenImage.Instance.FadeImage(
                    ResourceLoader.Load<Texture2D>("res://assets/cutscenes/door.jpg"),
                    1.5) 
                )
            }
        },

        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Por de trás dessa porta, há o que era você." },
            { "method", Callable.From( () =>
                FullScreenImage.Instance.ZoomImage(10, new Vector2I(91*5/4, 56*5/4), 5) 
                )
            }
        },

        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Não se deixe levar por algo tão vago como uma vida." },
            { "method", default }
        },

        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Lembre-se de porque você está aqui." },

            { "method", Callable.From( () => {
                FullScreenImage.Instance.FadeImage(ResourceLoader.Load<Texture2D>("res://assets/cutscenes/man.jpg"), 1.5);
                FullScreenImage.Instance.RemoveZoom(1.5);
                })
            }
        },


        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Você tem a sensação de familiaridade, como ao ver uma foto de muito tempo atrás, tão antiga que nem se lembra mais quando foi tirada." },
            { "method", Callable.From( () => FullScreenImage.Instance.FadeImage(null, 1.5) ) }
        },



        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Pela primeira vez desde que você chegou na sala, você consegue sentir suas mãos." },
            { "method", default }
        },


        new Dictionary {
            { "icon", "" },
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
    }},
    {"Win_2", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Você venceu novamente. Mas isso não há de trazer-te ganho algum, para nossa infelicidade." },
            { "method", Callable.From( () => FullScreenImage.Instance.ResetImage() ) }
        },

        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "O que verás em seguida, lembre-se, não é mais real. Ela foi importante para você... sim, preciosíssima. Não mais." },
            { "method",default}
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/sad.png" },
            { "title", "Israel" },
            { "dialog", "..." },
            { "method", Callable.From( () =>
                FullScreenImage.Instance.FadeImage(
                    ResourceLoader.Load<Texture2D>("res://assets/cutscenes/walk.jpg"),
                    1.5))
            }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Você era uma boa pessoa. Não era de exageros. Sagaz, porém gentil. Fará falta. Seus pais tinham muito orgulho do homem que você se tornou." },
            { "method",default}
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "???" },
            { "dialog", "Sua presença tornava a vida das pessoas ao seu redor melhor, na medida do possível. Sinto muito pela sua perda, mas agora… é hora de seguir em frente." },
            { "method",default}
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Mas você não quer seguir em frente, quer? Você não lembra exatamente quanto tempo faz que ela ocorreu, mas é como se você ainda estivesse lá. " },
            { "method",default}
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "A sensação da areia; da maresia; a temperatura começando a esfriar conforme o dia acaba. A pessoa que você mais ama, caminhando ao teu lado. Você gostaria que pudesse durar para sempre.  " },
            { "method",default }
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Você sente seu coração voltar a bater." },
            { "method", Callable.From( () =>
                FullScreenImage.Instance.FadeImage(null, 1.5))
            }
        }
    }},
    {"Win_3", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "A Morte" },
            { "dialog", "Você venceu, afinal." },
            { "method", Callable.From( () => FullScreenImage.Instance.ResetImage() ) }
        },

        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "A Morte" },
            { "dialog", "... Desculpe, criança, pois falhei em proteger-te. Vãos foram meus esforços, pois tu mesmo quisestes este fim. Agora, deves abraça-lo de acordo. " },
            { "method", default }
        }
    }
    },
    {"End_2", new Array<Dictionary>
    {
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Você sente dor." },
            { "method", Callable.From(
                () => {
                    FullScreenImage.Instance.ResetImage();
                    FullScreenImage.Instance.FadeColor(Colors.Black, 0.01);
                })
            }
        },

        new Dictionary {
            { "icon", "res://assets/sprites/portraits/tense.png" },
            { "title", "" },
            { "dialog", "Eu... Eu estava no banco do motorista. Meu coração batia rápido, mas meu pescoço não se mexia..." },
            { "method",default }
        },

        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Você não está mais mais naquela sala." },
            { "method", Callable.From( () => {
                FullScreenImage.Instance.FadeImage(
                    ResourceLoader.Load<Texture2D>("res://assets/cutscenes/accident.jpg"), 1.5);
                FullScreenImage.Instance.ZoomImage(10, new Vector2I(0, 320), 0.01);
                })
            }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/tense.png" },
            { "title", "Israel" },
            { "dialog", "Como- [shake rate=20.0 level=5 connected=1][b]Como eu pude vacilar assim?  [/b][/shake]" },
            { "method", Callable.From( () => {
                FullScreenImage.Instance.ZoomImage(6, new Vector2I(0, 160), 5);
                })
            }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/sad.png" },
            { "title", "Israel" },
            { "dialog", "Um erro tão pequeno, levando a uma perda tão grande… Se ao menos eu tivesse tomado mais cuidado... prestado mais atenção" },
            { "method", Callable.From( () => {
                FullScreenImage.Instance.ZoomImage(1, new Vector2I(0, 0), 5);
                })
            }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/sad.png" },
            { "title", "Israel" },
            { "dialog", "Talvez nós ainda... [i]É tudo culpa minha[/i]." },
            { "method", Callable.From( () => FullScreenImage.Instance.FadeColor(Colors.Black, 1.5) ) }
        },
        new Dictionary {
            { "icon", "res://assets/sprites/portraits/eye.png" },
            { "title", "A Morte" },
            { "dialog", "Agora, preso ao mundo mortal, sinto muito, mas não tenho mais como levar-te comigo. Nada posso fazer, pois este foi o exercício de [i]teu[/i] livre arbítrio." },
            { "method", default }
        },
        new Dictionary {
            { "icon", "" },
            { "title", "" },
            { "dialog", "Seu coração para." },
            { "method", Callable.From( () => {
                FullScreenImage.Instance.FadeColor(Colors.Red, 1.5);
                Task.Delay(3000).ContinueWith((Task t) => GameManager.Credits());
            }) }
        }
    }}
};
}
