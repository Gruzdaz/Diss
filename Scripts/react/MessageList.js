import React, { Component } from 'react';

class MessageList extends Component {


    componentDidMount() {
        this.scrollToBottom();
    }

    componentDidUpdate() {
        this.scrollToBottom();
    }

    scrollToBottom() {
        this.messagesEnd.scrollIntoView({ behavior: "smooth" });
    }

    render() {
        return (
                <div id="MList">
                <ul className="message-list">
                    {this.props.messages.map((message, index) => {
                        return (
                        <li key={index}>
                            <h4 className="message-sender">{message.senderId}</h4>
                            <p className="message-text">{message.text}</p>
                        </li>
                        )
                    })}
                    <div style={{ float: "left", clear: "both" }}
                        ref={(el) => { this.messagesEnd = el; }}>
                    </div>
                </ul>
                </div>
        );
    }
}
export default MessageList;