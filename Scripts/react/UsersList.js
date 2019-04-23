import React, { Component } from 'react';

class UsersList extends Component {
    render() {
            return (
                <div id="UList">
                    <ul className="users-list">
                        {this.props.users.map((user, index) => {
                            return (
                                <li key={index}>
                                    <span>{user.name}</span>
                                </li>
                            );
                        })}
                    </ul>
                </div>
            );
        }
    }
export default UsersList;