import React from "react";
import ReactDOM from "react-dom";
import ReactModal from 'react-modal';
import Chatkit from "@pusher/chatkit";
import MessageList from "./MessageList";
import SendMessageForm from "./SendMessageForm";
import Switch from 'react-toggle-switch';
import { BrowserRouter as Router, Route } from 'react-router-dom';
require("./node_modules/react-toggle-switch/dist/css/switch.min.css"); 

const testToken = "https://us1.pusherplatform.io/services/chatkit_token_provider/v1/9d2504dc-ba0c-43b8-ab5b-70d5c16da851/token";
const instanceLocator = "v1:us1:9d2504dc-ba0c-43b8-ab5b-70d5c16da851";
const userId = 'Irmantas';

const customStyles = {
    content: {
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        marginRight: '-50%',
        transform: 'translate(-50%, -50%)'
    }
};

ReactModal.setAppElement('#root');

class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            messages: [],
            events: [],
            roomId: 0,
            showModal: true,
            switched: true,
            username: ""
        };
        this.sendMessage = this.sendMessage.bind(this);
    }

    toggleSwitch() {
        this.setState(prevState => {
            return {
                switched: !prevState.switched
            };
        });
    }

    sendMessage(text) {
        this.currentUser.sendMessage({
            text,
            roomId: this.state.roomId
        });
 
    }

    handleCloseModal() {
        this.setState({ showModal: false });
        this.connect();
    }

    handleChange(event) {
        this.setState({username: event.target.value});
    }

    connect() {
        fetch('http://localhost:57971/events').then(res => res.json()).then(events =>
            this.setState({ events: events }));

        var tokenProvider = new Chatkit.TokenProvider({
            url: testToken
        });

        var Chatkit = require('./node_modules/@pusher/platform-node/target/index');

        const chatkit = new Chatkit.default({

            instanceLocator: 'v1:us1:example',

            key: 'your:key'

        });

        chatkit.createUser({

            id: 'extender',

            name: 'It is an extender',

        })

            .then((user) => {

                console.log('Success', user);

            }).catch((err) => {

                console.log(err);

            });

        const chatManager = new Chatkit.ChatManager({
            instanceLocator: instanceLocator,
            userId: "Irmantas",
            tokenProvider: tokenProvider
        });

        fetch('https://us1.pusherplatform.io/services/chatkit/v3/9d2504dc-ba0c-43b8-ab5b-70d5c16da851/users', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': tokenProvider

            },
            body: JSON.stringify({ "name": "John Doe", "id": "john", "avatar_url": "https://gravatar.com/img/2124", "custom_data": { "email": "john@example.com" } })
        });

        chatManager.connect()
            .then(currentUser => {
                this.currentUser = currentUser;
                if (!this.state.events.find(item => item.EventID === Number(this.props.match.params.eventId))) {
                    console.log("AAA");
                    this.currentUser.createRoom({
                        name: this.props.match.params.title,
                        private: true,
                        addUserIds: [this.state.username]
                    }).then(room => {
                        console.log("BBBB");
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
                    this.setState({ roomId: this.state.events.find(item => item.EventID === Number(this.props.match.params.eventId)).ChatID });
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

    render() {
        return (
            <div className="app">
                <p className="title"> {this.props.match.params.title}</p>
                <MessageList
                    roomId={this.state.roomId}
                    messages={this.state.messages}
                />
                <SendMessageForm
                    sendMessage={this.sendMessage}
                />

                <ReactModal
                    isOpen={this.state.showModal}
                    contentLabel="Modal #1 Global Style Override Example"
                    onRequestClose={this.handleCloseModal.bind(this)}
                    style={customStyles}
                >
                    <div>
                        <label>
                            Display Name: 
                            <input onChange={this.handleChange.bind(this)} className="ml-1 form-group" type="text" name="name" />
                        </label>
                    </div>
                        <label>
                            Match with opponent team fans: 
                        </label>
                    <Switch className="align-middle ml-1" onClick={this.toggleSwitch.bind(this)} on={this.state.switched} />
                    <div>
                        <button className="btn btn-primary" onClick={this.handleCloseModal.bind(this)}>Submit</button>
                    </div>
                </ReactModal>
            </div>

        );
    }
}

ReactDOM.render(
    <Router>
        <Route path='/chat/:eventId/:title' component={App} />
    </Router>,
    document.getElementById('root'));
