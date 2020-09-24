
window.ytPlayer = {
    initialized: false,
    videoQueued: false,
    player: null,
    isReady: function() {
        return this.initialized;
    },
    loadApi: function() {
        // This code loads the IFrame Player API code asynchronously.
        var tag = document.createElement('script');

        tag.src = "https://www.youtube.com/iframe_api";
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
    },

    playVideo: function () {
        //if (this.initialized && this.player.getPlayerState() === YT.PlayerState.PLAYING) {
        //    return;
        //}

        if (this.initialized && (this.player.getPlayerState() === YT.PlayerState.CUED || this.player.getPlayerState() === YT.PlayerState.PAUSED)) {
            this.player.playVideo();
        }
        else {
            this.videoQueued = true;
        }
    },

    createPlayer: function (element) {
        if (this.player) {
            console.log('YouTube player is already initialized.');
            return;
        }

        this.loadApi();

        var self = this;
        var onPlayerReady = function() {
            self.initialized = true;
            console.log("YouTube player initialized.");
        }

        var onPlayerStateChange = function (event) {
            console.log('YouTube player state changed: ' + event.data);

            if (self.videoQueued && event.data === YT.PlayerState.CUED) {
                self.videoQueued = false;
                self.player.playVideo();
            }

            if (event.data === YT.PlayerState.ENDED) {
                DotNet.invokeMethodAsync('Jukebox.Client', 'Controls.Next');
            }
        }

        window.onYouTubeIframeAPIReady = function() {
            self.player = new YT.Player(element,
                {
                    videoId: '',
                    events: {
                        'onReady': onPlayerReady,
                        'onStateChange': onPlayerStateChange
                    },
                    playerVars: {
                        autoplay: 0,
                        controls: 0,
                        disablekb: 1,
                        enablejsapi: 1
                    }
                });
            console.log("YouTube player created.");
        }
    }
}