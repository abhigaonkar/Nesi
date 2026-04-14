import { Component, OnInit } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import * as theme from "./themes";

@Component({
  selector: 'app-theme',
  templateUrl: './theme.component.html',
  styleUrls: ['./theme.component.css']
})
export class ThemeComponent implements OnInit {

  private themeElement: Element = document.getElementById('theme_styles');

  public selectedTheme: string = 'customTheme';

  constructor() {
    this.themeElement.innerHTML = theme[this.selectedTheme];

  }

  ngOnInit() { }

  onThemeChange() {
    if (this.selectedTheme === 'bootstrapTheme') {
      this.themeElement.innerHTML = theme.bootstrapTheme;
    } else if (this.selectedTheme === 'cruzeTheme') {
      this.themeElement.innerHTML = theme.cruzeTheme;
    } else if (this.selectedTheme === 'cupertinoTheme') {
      this.themeElement.innerHTML = theme.cupertinoTheme;
    } else if (this.selectedTheme === 'darknessTheme') {
      this.themeElement.innerHTML = theme.darknessTheme;
    } else if (this.selectedTheme === 'flickTheme') {
      this.themeElement.innerHTML = theme.flickTheme;
    } else if (this.selectedTheme === 'homeTheme') {
      this.themeElement.innerHTML = theme.homeTheme;
    } else if (this.selectedTheme === 'kasperTheme') {
      this.themeElement.innerHTML = theme.kasperTheme;
    } else if (this.selectedTheme === 'lightnessTheme') {
      this.themeElement.innerHTML = theme.lightnessTheme;
    } else if (this.selectedTheme === 'ludvigTheme') {
      this.themeElement.innerHTML = theme.ludvigTheme;
    } else if (this.selectedTheme === 'omegaTheme') {
      this.themeElement.innerHTML = theme.omegaTheme;
    } else if (this.selectedTheme === 'redmondTheme') {
      this.themeElement.innerHTML = theme.redmondTheme;
    } else if (this.selectedTheme === 'rocketTheme') {
      this.themeElement.innerHTML = theme.rocketTheme;
    } else if (this.selectedTheme === 'startTheme') {
      this.themeElement.innerHTML = theme.startTheme;
    } else if (this.selectedTheme === 'trontasticTheme') {
      this.themeElement.innerHTML = theme.trontasticTheme;
    } else if (this.selectedTheme === 'voclainTheme') {
      this.themeElement.innerHTML = theme.voclainTheme;
    } else {
      this.themeElement.innerHTML = theme.customTheme;
    }
  }

}
