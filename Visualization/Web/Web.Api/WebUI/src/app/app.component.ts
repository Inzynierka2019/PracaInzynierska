import { Component, HostListener } from '@angular/core';
import * as $ from 'jquery';
import { LogsService } from './services/logs.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  styles: [`.router-link-active { background-color: red; }`]
})
export class AppComponent {
  title = 'Visualization';

  constructor(private logsService: LogsService) {
    this.logsService.init();
  }

  @HostListener('window:scroll')
  onWindowScroll() {
    this.animateNavBar();
    this.animateScrollTop();
  }

  onPageUp() {
    $('html').animate({ scrollTop: 0 }, {
      duration: 400,
      queue: false,
    }, "swing");
  }

  animateNavBar() {
    const navbar = $('nav');
    const hasShowOffClass = navbar.hasClass('scroll-effect');
    const windowOffset = window.pageYOffset;
    const targetScaleInClass = windowOffset > 1 ? true : false;

    if (!hasShowOffClass && targetScaleInClass) {
      navbar.addClass('scroll-effect');
    }

    if (hasShowOffClass && !targetScaleInClass) {
      navbar.removeClass('scroll-effect');
    }
  }

  animateScrollTop() {
    const scrollTop = $('.scroll-btn');
    const windowOffset = window.pageYOffset;
    const targetScaleInClass = windowOffset > 100 ? true : false;

    if (!targetScaleInClass) {
      scrollTop.addClass('scale-in');
    }

    if (targetScaleInClass) {
      scrollTop.removeClass('scale-in');
    }
  }
}