import { User } from './../../models/User';

import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  users: User[] = []
  constructor(private UserService: UsersService) {  }
  

  ngOnInit(): void {
    this.UserService.getUsers().subscribe(users =>{
      this.users = users;
    })
  }


}
