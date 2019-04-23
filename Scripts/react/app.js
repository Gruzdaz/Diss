import SendMessageForm from "./SendMessageForm";
import UsersList from "./UsersList";
import React, { Component } from 'react';
import ReactDOM from "react-dom";
import ReactModal from 'react-modal';
import Switch from 'react-toggle-switch';
import { ChatManager, TokenProvider } from '@pusher/chatkit-client';
import { default as Chatkit } from '@pusher/chatkit-server';
import MessageList from './MessageList';
import { BrowserRouter as Router, Route } from 'react-router-dom';
require("./node_modules/react-toggle-switch/dist/css/switch.min.css"); 


const testToken = "https://us1.pusherplatform.io/services/chatkit_token_provider/v1/9d2504dc-ba0c-43b8-ab5b-70d5c16da851/token";
const instanceLocator = "v1:us1:9d2504dc-ba0c-43b8-ab5b-70d5c16da851";

const chatkit = new Chatkit({
    instanceLocator: "v1:us1:9d2504dc-ba0c-43b8-ab5b-70d5c16da851",
    key: "9e0b93b8-23d6-4ec4-992f-2d7c561a2616:1xhmQyqlYrXa2AYfmrUJfijFJg4Df5cCtQT753fLtuk="
});

const tokenProvider = new TokenProvider({
    url: testToken
});

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

class App extends Component {
    constructor(props) {
        super(props);
        this.state = {
            messages: [],
            events: [],
            roomId: 0,
            username: "",
            showModal: true,
            switched: true,
            switchedHome: true,
            currentRoom: [],
            users: []
        };
        this.addMessage = this.addMessage.bind(this);
    }

    toggleSwitch() {
        this.setState(prevState => {
            return {
                switched: !prevState.switched
            };
        });
    }

    toggleSwitchHome() {
        this.setState(prevState => {
            return {
                switchedHome: !prevState.switchedHome
            };
        });
    }

    addMessage(text) {
        this.state.currentUser.sendMessage({
            text,
            roomId: this.state.roomId.toString()
        });
    }

    handleCloseModal() {
        this.setState({ showModal: false });
        this.connect();
    }

    createManager(name) {
        fetch('http://localhost:57971/events').then(res => res.json()).then(events =>
            this.setState({ events: events }));

        const chatManager = new ChatManager({
            instanceLocator: instanceLocator,
            userId: name,
            tokenProvider: tokenProvider
        });

        chatManager.connect()
            .then(currentUser => {
                this.currentUser = currentUser;
                if (!this.state.events.find(item => item.EventID === Number(this.props.match.params.eventId) && item.MixedRoom === this.state.switched && item.HomeTeamRoom === this.state.switchedHome)) {
                    this.setState({
                        currentUser: currentUser,
                        users: currentUser.users
                    });
                    this.currentUser.createRoom({
                        name: this.props.match.params.title,
                        private: false,
                        addUserIds: [currentUser.id]
                    }).then(room => {
                        this.setState({ roomId: room.id.toString(), currentRoom: room });
                        fetch('http://localhost:57971/chatID', {
                            method: 'post',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify({ "ChatID": Number(room.id), "ID": Number(this.props.match.params.eventId), "MixedRoom": this.state.switched, "HomeTeamRoom": this.state.switchedHome })
                        }).then(this.currentUser.subscribeToRoom(
                            {
                                roomId: this.state.roomId.toString(),
                                hooks: {
                                    onMessage: message => {

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
                    this.setState({
                        currentUser: this.currentUser,
                        users: this.currentUser.users
                    });

                    fetch('http://localhost:57971/MatchRoom', {
                        method: 'post',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({ "SupportsHomeTeam": this.state.switchedHome, "AllowsMixedRooms": this.state.switched, "EventID": Number(this.props.match.params.eventId) })
                    })
                        .then(res => res.json()).then(roomID => {
                            this.setState({
                                roomId: roomID.toString()
                            });
                            this.joinRoom(this.state.roomId.toString());
                        });
                }
            });
    }

    updateChat() {
        if (this.state.currentRoom.member_user_ids === undefined) {
            setTimeout(this.updateChat, 1500);
        }
        fetch('http://localhost:57971/UpdateChat', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ "ChatID": Number(this.state.roomId), "EventID": Number(this.props.match.params.eventId), "Users": this.state.currentRoom.member_user_ids })
        });
    }

    joinRoom(roomId) {
        console.log("JOIN:" + roomId);
        chatkit.getRoom({
            roomId: roomId.toString()
        }).then(room => {
            this.setState({ currentRoom: room });
            console.log("ROOM:" + roomId);
            //this.setState({ users: room.member_user_ids });
            this.updateChat();
        });

        this.state.currentUser.subscribeToRoom({
                roomId: this.state.roomId,
                hooks: {
                    onMessage: message => {
                        this.setState({
                            messages: [...this.state.messages, message],
                            users: this.state.currentUser.users
                        });
                    }
                }
            }).then(currentRoom => {
            this.setState({
                currentRoom
            });
        })
        .catch(error => console.log(error));
    }

    createUser(name) {
        fetch('http://localhost:57971/Users').then(res => res.json()).then(users => {
                chatkit.createUser({
                    id: name,
                    name: name
                }).catch((err) => {
                    console.log(err.status);
                }
                ).then(currentUser => {
                    this.setState(currentUser);
                    this.createManager(name);
                });
        });
    }

    connect() {

        fetch('http://localhost:57971/currentUser').then(res => res.json()).then(user =>
            this.createUser(user));
    }


    render() {
            return (
                <div className="app">
                    <p className="title"> {this.props.match.params.title}</p>
                    <UsersList
                        users={this.state.users}
                    />
                    <MessageList
                        messages={this.state.messages}
                    />
                    <SendMessageForm
                        sendMessage={this.addMessage}

                    />
                <ReactModal
                    isOpen={this.state.showModal}
                    contentLabel="Modal #1 Global Style Override Example"
                    onRequestClose={this.handleCloseModal.bind(this)}
                    style={customStyles}
                >
                <div>
                <label>
                    Supporting home team:
                </label>
                <Switch className="align-middle ml-1 pull-right" onClick={this.toggleSwitchHome.bind(this)} on={this.state.switchedHome} />
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
