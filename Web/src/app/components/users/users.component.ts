import { User } from './../../models/User';

import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  @Input() user!: User;
  constructor() {}

  ngOnInit(): void {
    console.log(this.user)
  }

}
