import React from "react";
import ReactDOM from "react-dom";
import Chatkit from "@pusher/chatkit";
import MessageList from "./MessageList";
import SendMessageForm from "./SendMessageForm";

const testToken = "https://us1.pusherplatform.io/services/chatkit_token_provider/v1/9d2504dc-ba0c-43b8-ab5b-70d5c16da851/token";
const instanceLocator = "v1:us1:9d2504dc-ba0c-43b8-ab5b-70d5c16da851";
const roomId = 19401803;
const username = 'perborgen';

class App extends React.Component {
    constructor() {
        super();
        this.state = {
            messages: []
        };
        this.sendMessage = this.sendMessage.bind(this);
    }

    componentDidMount() {
        const chatManager = new Chatkit.ChatManager({
            instanceLocator: instanceLocator,
            userId: 'Irmantas',
            tokenProvider: new Chatkit.TokenProvider({
                url: testToken
            })
        });

        chatManager.connect()
            .then(currentUser => {
                this.currentUser = currentUser;
                this.currentUser.subscribeToRoom({
                    roomId: roomId,
                    hooks: {
                        onNewMessage: message => {

                            this.setState({
                                messages: [...this.state.messages, message]
                            });
                        }
                    }
                });
            });
    }

    sendMessage(text) {
        this.currentUser.sendMessage({
            text,
            roomId: roomId
        });
    }

    render() {
        return (
            <div className="app">
                <Title />
                <MessageList
                    roomId={this.state.roomId}
                    messages={this.state.messages}
                />
                <SendMessageForm
                    sendMessage={this.sendMessage}
                />
            </div>
        );
    }
}

function Title() {
    return <p className="title">Lakers vs Boston</p>
}

ReactDOM.render(<App />, document.getElementById('root'));
