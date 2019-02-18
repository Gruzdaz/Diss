import React from "react";
import ReactDOM from "react-dom";
//import { Router, Route, browserHistory} from 'react-router';
import Chatkit from "@pusher/chatkit";
import MessageList from "./MessageList";
import SendMessageForm from "./SendMessageForm";
import { BrowserRouter as Router, Route} from 'react-router-dom';

const testToken = "https://us1.pusherplatform.io/services/chatkit_token_provider/v1/9d2504dc-ba0c-43b8-ab5b-70d5c16da851/token";
const instanceLocator = "v1:us1:9d2504dc-ba0c-43b8-ab5b-70d5c16da851";
//const roomId = 19401803;
const userId = 'Irmantas';

class App extends React.Component {
    constructor() {
        super();
        this.state = {
            messages: [],
            events: [],
            roomId: 0
        };
        this.sendMessage = this.sendMessage.bind(this);
    }

    componentDidMount() {
        fetch('http://localhost:57971/events').then(res => res.json()).then(events =>
            this.setState({ events }));
        const chatManager = new Chatkit.ChatManager({
            instanceLocator: instanceLocator,
            userId: userId,
            tokenProvider: new Chatkit.TokenProvider({
                url: testToken
            })
        });

        chatManager.connect()
            .then(currentUser => {
                this.currentUser = currentUser;
                if (!this.state.events.find(item => item.ID === Number(this.props.match.params.eventId)).ChatID) {
                    console.log("AAA");
                    this.currentUser.createRoom({
                        name: this.state.events.find(item => item.ID === Number(this.props.match.params.eventId)).Name,
                        private: true,
                        addUserIds: ['Irmantas']
                    }).then(room => {
                        console.log("BBB");
                        this.setState({ roomId: room.id });
                        fetch('http://localhost:57971/chatID', {
                            method: 'post',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify({ "ChatID": Number(room.id), "ID": Number(this.props.match.params.eventId) })
                        }).then(this.currentUser.subscribeToRoom(
                            {
                                roomId: this.state.roomId,
                                hooks: {
                                    onNewMessage: message => {

                                        this.setState({
                                            messages: [...this.state.messages, message]
                                        });
                                    }
                                }
                            }));
                        console.log(`Created room called ${room.id}`);
                    })
                        .catch(err => {
                            console.log(`Error creating room ${err}`);
                        });
                }
                else {
                    this.setState({ roomId: this.state.events.find(item => item.ID === Number(this.props.match.params.eventId)).ChatID });
                    this.currentUser.subscribeToRoom(
                        {
                            roomId: this.state.roomId,
                            hooks: {
                                onNewMessage: message => {

                                    this.setState({
                                        messages: [...this.state.messages, message]
                                    });
                                }
                            }
                        });
                }
            });
    }

    sendMessage(text) {
        this.currentUser.sendMessage({
            text,
            roomId: this.state.roomId
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

ReactDOM.render(
    <Router>
        <Route path='/chat/:eventId' component={App} />
    </Router>,
    document.getElementById('root'));
